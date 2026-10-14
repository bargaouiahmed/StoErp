namespace StoErp.Shared.Application.Events;

public interface IOutboxEventReaderStrategy
{
    bool CanHandle(string eventType);

    IEvent DeserializeEvent(OutboxEntry outboxEntry);
}
