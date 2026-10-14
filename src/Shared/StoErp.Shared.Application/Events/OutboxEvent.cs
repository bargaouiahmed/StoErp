namespace StoErp.Shared.Application.Events;

public sealed record OutboxEvent(
    OutboxEntry Entry,
    IEvent Event);
