using StoErp.Shared.Application.Events;

namespace StoErp.Modules.Auth.Application.Ports.Outbox;

public interface IAuthOutboxRepository
{
    void Add(OutboxEntry entry);

    Task<IReadOnlyList<OutboxEntry>> ReadPendingBatchAsync(
        int batchSize,
        DateTimeOffset now,
        CancellationToken cancellationToken = default);

    Task MarkAsProcessedAsync(
        Guid messageId,
        DateTimeOffset processedAt,
        CancellationToken cancellationToken = default);

    Task MarkAsFailedAsync(
        Guid messageId,
        string errorMessage,
        DateTimeOffset nextAttemptAt,
        CancellationToken cancellationToken = default);
    Task MarkAsDeadAsync(
        Guid messageId,
        string errorMessage,
        DateTimeOffset deadAt,
        CancellationToken cancellationToken = default);
}
