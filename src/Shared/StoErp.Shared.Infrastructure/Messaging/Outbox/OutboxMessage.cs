
using System.Text.Json;

namespace StoErp.Shared.Infrastructure.Messaging.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; private set; }
    public string EventType { get; private set; } = null!;
    public JsonDocument Payload { get; private set; } = null!;
    public DateTimeOffset OccurredAt { get; private set; }
    public DateTimeOffset? ProcessedAt { get; private set; }
    public int AttemptCount { get; private set; }
    public DateTimeOffset? NextAttemptAt { get; private set; }
    public DateTimeOffset? DeadAt { get; private set; }
    public string[] CompletedHandlers { get; private set; } = [];
    public JsonDocument? ErrorHistory { get; private set; }
    private OutboxMessage() { }

    public OutboxMessage(Guid id, string eventType, JsonDocument eventData, DateTimeOffset occurredAt)
    {
        Id = id;
        EventType = eventType;
        Payload = eventData;
        OccurredAt = occurredAt;
        AttemptCount = 0;
    }

    public void MarkProcessed(DateTimeOffset processedAt)
    {
        if (ProcessedAt != null)
        {
            throw new InvalidOperationException("Message has already been processed.");
        }
        ProcessedAt = processedAt;
        NextAttemptAt = null;

    }
    public void RecordCompletedHandlers(IEnumerable<string> handlerNames)
    {
        CompletedHandlers =
        [
            .. CompletedHandlers.Union(handlerNames, StringComparer.Ordinal)
        ];
    }
    public void RecordFailure(string errorMessage, DateTimeOffset nextAttemptAt)
    {

        AttemptCount++;
        AddErrorToHistory(errorMessage, AttemptCount);
        NextAttemptAt = nextAttemptAt;
    }
    public void MarkDead(DateTimeOffset deadAt, string errorMessage)
    {
        AttemptCount++;

        AddErrorToHistory(errorMessage, AttemptCount);

        DeadAt = deadAt;
        NextAttemptAt = null;

    }
    private void AddErrorToHistory(string errorMessage, int attemptNumber)
    {
        var errorEntry = new
        {
            AttemptNumber = attemptNumber,
            ErrorMessage = errorMessage,
            Timestamp = DateTimeOffset.UtcNow
        };

        var previousHistory = ErrorHistory;
        var existingErrors = previousHistory is null
            ? new List<JsonElement>()
            : previousHistory.RootElement.EnumerateArray().ToList();

        existingErrors.Add(JsonSerializer.SerializeToElement(errorEntry));

        ErrorHistory = JsonDocument.Parse(JsonSerializer.Serialize(existingErrors));
        previousHistory?.Dispose();
    }



}
