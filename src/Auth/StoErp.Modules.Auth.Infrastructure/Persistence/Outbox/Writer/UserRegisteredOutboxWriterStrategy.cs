
using System.Text.Json;
using StoErp.Shared.Application.Events;
using StoErp.Shared.Contracts.AuthModuleContracts;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Outbox.Writer;

public sealed class UserRegisteredOutboxWriterStrategy : IOutboxEventWriterStrategy
{
    public bool CanHandle(IEvent eventMessage)
    {
        return eventMessage is UserRegisteredEvent;
    }

    public OutboxEntry CreateEntry(IEvent eventMessage)
    {
        if (eventMessage is not UserRegisteredEvent userRegisteredEvent)
        {
            throw new ArgumentException("Invalid event type. Expected UserRegisteredEvent.", nameof(eventMessage));
        }

        var payload = JsonDocument.Parse(JsonSerializer.Serialize(userRegisteredEvent));

        return new OutboxEntry(
            Guid.CreateVersion7(),
            nameof(UserRegisteredEvent),
            payload,
            DateTimeOffset.UtcNow,
            0
        );
    }

}
