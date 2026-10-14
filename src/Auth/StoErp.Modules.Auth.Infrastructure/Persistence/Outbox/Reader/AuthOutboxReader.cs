using StoErp.Shared.Application.Events;
using StoErp.Shared.Infrastructure.Messaging.Outbox;
using StoErp.Modules.Auth.Application.Ports.Persistence;
using StoErp.Modules.Auth.Application.Ports.Outbox;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Outbox.Reader;

public sealed class AuthOutboxReader : IOutboxReader
{
    private readonly IAuthOutboxRepository _outboxRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEnumerable<IOutboxEventReaderStrategy> _readerStrategies;


    public AuthOutboxReader(
        IAuthOutboxRepository outboxRepository,
        IUnitOfWork unitOfWork,
        IEnumerable<IOutboxEventReaderStrategy> readerStrategies)
    {
        _outboxRepository = outboxRepository;
        _unitOfWork = unitOfWork;
        _readerStrategies = readerStrategies;
    }

    public async Task<IReadOnlyList<OutboxEvent>> ReadBatchAsync(
        int batchSize = 50,
        CancellationToken cancellationToken = default)
    {
        if (batchSize <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(batchSize),
                "Batch size must be greater than zero.");
        }

        var entries = await _outboxRepository.ReadPendingBatchAsync(
            batchSize,
            DateTimeOffset.UtcNow,
            cancellationToken);

        var events = new List<OutboxEvent>(entries.Count);

        foreach (var entry in entries)
        {
            var strategy = _readerStrategies.SingleOrDefault(
                reader => reader.CanHandle(entry.EventType));

            if (strategy is null)
            {
                throw new InvalidOperationException(
                    $"No outbox reader strategy is registered for event type '{entry.EventType}'.");
            }

            events.Add(new OutboxEvent(
                entry,
                strategy.DeserializeEvent(entry)));
        }

        return events;
    }

    public async Task MarkAsProcessedAsync(
        Guid messageId,
        CancellationToken cancellationToken = default)
    {
        await _outboxRepository.MarkAsProcessedAsync(
            messageId,
            DateTimeOffset.UtcNow,
            cancellationToken);
    }

    public async Task MarkAsFailedAsync(
        Guid messageId,
        string errorMessage,
        DateTimeOffset nextAttemptAt,
        CancellationToken cancellationToken = default)
    {
        await _outboxRepository.MarkAsFailedAsync(
            messageId,
            errorMessage,
            nextAttemptAt,
            cancellationToken);
    }

    public async Task MarkAsDeadAsync(
        Guid messageId,
        string errorMessage,
        CancellationToken cancellationToken = default)
    {
        await _outboxRepository.MarkAsDeadAsync(
            messageId,
            errorMessage,
            DateTimeOffset.UtcNow,
            cancellationToken);
    }
    public Task CommitAsync(CancellationToken cancellationToken = default)
    {
        return _unitOfWork.CommitAsync(cancellationToken);
    }
}
