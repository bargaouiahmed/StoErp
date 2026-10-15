using Microsoft.Extensions.Logging;
using StoErp.Shared.Application.Events;

namespace StoErp.Shared.Infrastructure.Messaging.Outbox;

public sealed class OutboxDispatcher
{
    private readonly IEnumerable<IOutboxReader> _outboxReaders;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<OutboxDispatcher> _logger;

    public OutboxDispatcher(
        IEnumerable<IOutboxReader> outboxReaders,
        IEventPublisher eventPublisher,
        ILogger<OutboxDispatcher> logger)
    {
        _outboxReaders = outboxReaders;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    public async Task DispatchBatchAsync(
        int batchSize = 50,
        CancellationToken cancellationToken = default)
    {
        foreach (var reader in _outboxReaders)
        {
            await DispatchReaderBatchAsync(reader, batchSize, cancellationToken);
        }
    }

    private async Task DispatchReaderBatchAsync(
        IOutboxReader reader,
        int batchSize,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<OutboxEvent> outboxEvents;

        try
        {
            outboxEvents = await reader.ReadBatchAsync(batchSize, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Failed to read a batch of outbox events. Skipping this reader for the current tick.");
            return;
        }

        try
        {
            foreach (var outboxEvent in outboxEvents)
            {
                await ProcessOutboxEventAsync(reader, outboxEvent, cancellationToken);
            }
        }
        finally
        {
            try
            {
                await reader.CommitAsync(cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Failed to commit outbox state. The current batch will be redelivered.");
            }
        }
    }

    private async Task ProcessOutboxEventAsync(
        IOutboxReader reader,
        OutboxEvent outboxEvent,
        CancellationToken cancellationToken)
    {
        var newlyCompletedHandlers = new HashSet<string>(StringComparer.Ordinal);

        try
        {
            await PublishAsync(
                outboxEvent.Event,
                outboxEvent.Entry.CompletedHandlers,
                newlyCompletedHandlers,
                cancellationToken);

            await PersistCompletedHandlersAsync(
                reader,
                outboxEvent.Entry.Id,
                newlyCompletedHandlers,
                cancellationToken);

            await reader.MarkAsProcessedAsync(
                outboxEvent.Entry.Id,
                cancellationToken);
        }
        catch (Exception exception)
        {
            await PersistCompletedHandlersAsync(
                reader,
                outboxEvent.Entry.Id,
                newlyCompletedHandlers,
                cancellationToken);

            var nextAttemptCount = outboxEvent.Entry.AttemptCount + 1;

            try
            {
                if (OutboxRetryPolicy.ShouldDeadLetter(nextAttemptCount))
                {
                    await reader.MarkAsDeadAsync(
                        outboxEvent.Entry.Id,
                        exception.ToString(),
                        cancellationToken);
                }
                else
                {
                    await reader.MarkAsFailedAsync(
                        outboxEvent.Entry.Id,
                        exception.ToString(),
                        OutboxRetryPolicy.CalculateNextAttemptAt(
                            nextAttemptCount,
                            DateTimeOffset.UtcNow),
                        cancellationToken);
                }
            }
            catch (Exception markException)
            {
                _logger.LogError(
                    markException,
                    "Failed to record the outbox failure for message '{MessageId}'.",
                    outboxEvent.Entry.Id);
            }
        }
    }

    private async Task PersistCompletedHandlersAsync(
        IOutboxReader reader,
        Guid messageId,
        IReadOnlyCollection<string> handlerNames,
        CancellationToken cancellationToken)
    {
        if (handlerNames.Count == 0)
        {
            return;
        }

        try
        {
            await reader.MarkHandlersCompletedAsync(
                messageId,
                handlerNames,
                cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Failed to record completed handlers for message '{MessageId}'. "
                + "Already completed handlers may run again on the next attempt.",
                messageId);
        }
    }

    private async Task PublishAsync(
        IEvent eventMessage,
        IReadOnlyCollection<string> completedHandlers,
        ISet<string> newlyCompletedHandlers,
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
            new object?[]
            {
                eventMessage,
                completedHandlers,
                newlyCompletedHandlers,
                cancellationToken
            });

        if (task is null)
        {
            throw new InvalidOperationException(
                "The event publisher did not return a Task.");
        }

        await task;
    }
}
