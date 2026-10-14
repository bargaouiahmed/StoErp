using System.Text.Json;

namespace StoErp.Shared.Application.Events;

public sealed record OutboxEntry
(
    Guid Id,
    string EventType,
    JsonDocument Payload,
    DateTimeOffset OccurredAt,
    int AttemptCount
);
