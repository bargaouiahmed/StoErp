

namespace StoErp.Shared.Application.Events;

public interface IOutboxReader
{
    Task<IReadOnlyList<OutboxEvent>> ReadBatchAsync(
        int batchSize,
        CancellationToken cancellationToken = default);

    Task MarkAsProcessedAsync(
        Guid messageId,
        CancellationToken cancellationToken = default);

    Task MarkAsFailedAsync(
        Guid messageId,
        string errorMessage,
        DateTimeOffset nextAttemptAt,
        CancellationToken cancellationToken = default);
    Task MarkAsDeadAsync(
        Guid messageId,
        string errorMessage,
        CancellationToken cancellationToken = default);

    Task MarkHandlersCompletedAsync(
        Guid messageId,
        IReadOnlyCollection<string> handlerNames,
        CancellationToken cancellationToken = default);

    Task CommitAsync(
        CancellationToken cancellationToken = default);
}
