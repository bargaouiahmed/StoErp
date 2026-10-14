using StoErp.Shared.Application.Events;

namespace StoErp.Shared.Infrastructure.Messaging.Outbox;

public sealed class OutboxDispatcher
{
    private readonly IEnumerable<IOutboxReader> _outboxReaders;
    private readonly IEventPublisher _eventPublisher;

    public OutboxDispatcher(
        IEnumerable<IOutboxReader> outboxReaders,
        IEventPublisher eventPublisher)
    {
        _outboxReaders = outboxReaders;
        _eventPublisher = eventPublisher;
    }

    public async Task DispatchBatchAsync(
        int batchSize = 50,
        CancellationToken cancellationToken = default)
    {
        foreach (var reader in _outboxReaders)
        {
            var outboxEvents = await reader.ReadBatchAsync(
                batchSize,
                cancellationToken);

            foreach (var outboxEvent in outboxEvents)
            {
                try
                {
                    await PublishAsync(
                        outboxEvent.Event,
                        cancellationToken);

                    await reader.MarkAsProcessedAsync(
                        outboxEvent.Entry.Id,
                        cancellationToken);
                }
                catch (Exception exception)
                {
                    var nextAttemptAt =
                        OutboxRetryPolicy.CalculateNextAttemptAt(
                            outboxEvent.Entry.AttemptCount + 1,
                            DateTimeOffset.UtcNow);
                    if (OutboxRetryPolicy.ShouldDeadLetter(outboxEvent.Entry.AttemptCount + 1))
                    {
                        await reader.MarkAsDeadAsync(
                            outboxEvent.Entry.Id,
                            exception.ToString(),
                            cancellationToken);
                        continue;
                    }
                    else
                    {
                        await reader.MarkAsFailedAsync(
                            outboxEvent.Entry.Id,
                            exception.ToString(),
                            nextAttemptAt,
                            cancellationToken);
                    }
                }
            }

            await reader.CommitAsync(cancellationToken);
        }
    }

    internal async Task DispatchPendingMessagesAsync(int batchSize, CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }

    private async Task PublishAsync(
        IEvent eventMessage,
        CancellationToken cancellationToken)
    {
        var method = typeof(IEventPublisher)
            .GetMethod(nameof(IEventPublisher.PublishAsync))
            ?? throw new InvalidOperationException(
                "IEventPublisher.PublishAsync was not found.");

        var genericMethod = method.MakeGenericMethod(
            eventMessage.GetType());

        var task = (Task?)genericMethod.Invoke(
            _eventPublisher,
            new object?[] { eventMessage, cancellationToken });

        if (task is null)
        {
            throw new InvalidOperationException(
                "The event publisher did not return a Task.");
        }

        await task;
    }
}
