using Microsoft.EntityFrameworkCore;
using StoErp.Modules.Auth.Application.Ports.Outbox;
using StoErp.Shared.Application.Events;
using StoErp.Shared.Infrastructure.Messaging.Outbox;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Outbox.Repositories;

public sealed class AuthOutboxRepository : IAuthOutboxRepository
{
    private readonly AuthDbContext _dbContext;

    public AuthOutboxRepository(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(OutboxEntry entry)
    {
        _dbContext.OutboxMessages.Add(
            new OutboxMessage(
                entry.Id,
                entry.EventType,
                entry.Payload,
                entry.OccurredAt));
    }

    public async Task<IReadOnlyList<OutboxEntry>> ReadPendingBatchAsync(
        int batchSize,
        DateTimeOffset now,
        CancellationToken cancellationToken = default)
    {
        if (batchSize <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(batchSize),
                "Batch size must be greater than zero.");
        }

        return await _dbContext.OutboxMessages
            .Where(message =>
                message.ProcessedAt == null && message.DeadAt == null
                && (message.NextAttemptAt == null
                    || message.NextAttemptAt <= now))
            .OrderBy(message => message.OccurredAt)
            .Take(batchSize)
            .Select(message => new OutboxEntry(
                message.Id,
                message.EventType,
                message.Payload,
                message.OccurredAt,
                message.AttemptCount)
            {
                CompletedHandlers = message.CompletedHandlers
            })
            .ToListAsync(cancellationToken);
    }

    public async Task MarkHandlersCompletedAsync(
        Guid messageId,
        IReadOnlyCollection<string> handlerNames,
        CancellationToken cancellationToken = default)
    {
        if (handlerNames.Count == 0)
        {
            return;
        }

        var message = await FindMessageAsync(messageId, cancellationToken);
        message.RecordCompletedHandlers(handlerNames);
    }

    public async Task MarkAsProcessedAsync(
        Guid messageId,
        DateTimeOffset processedAt,
        CancellationToken cancellationToken = default)
    {
        var message = await FindMessageAsync(messageId, cancellationToken);
        message.MarkProcessed(processedAt);
    }

    public async Task MarkAsFailedAsync(
        Guid messageId,
        string errorMessage,
        DateTimeOffset nextAttemptAt,
        CancellationToken cancellationToken = default)
    {
        var message = await FindMessageAsync(messageId, cancellationToken);
        message.RecordFailure(errorMessage, nextAttemptAt);
    }

    public async Task MarkAsDeadAsync(
        Guid messageId,
        string errorMessage,
        DateTimeOffset deadAt,
        CancellationToken cancellationToken = default)
    {
        var message = await FindMessageAsync(messageId, cancellationToken);
        message.MarkDead(deadAt, errorMessage);
    }
    private async Task<OutboxMessage> FindMessageAsync(
        Guid messageId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.OutboxMessages
            .SingleOrDefaultAsync(
                message => message.Id == messageId,
                cancellationToken)
            ?? throw new InvalidOperationException(
                $"Outbox message '{messageId}' was not found.");
    }
}
