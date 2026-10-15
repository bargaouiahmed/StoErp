
using System.Text.Json;
using StoErp.Shared.Application.Events;
using StoErp.Shared.Contracts.AuthModuleContracts;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Outbox.Writer;

public class UserRequestedPasswordResetOutboxWriterStrategy : IOutboxEventWriterStrategy
{
    public bool CanHandle(IEvent eventMessage)
    {
        return eventMessage is UserRequestedChangePassword;
    }

    public OutboxEntry CreateEntry(IEvent eventMessage)
    {
        if(eventMessage is not UserRequestedChangePassword userRequestedChangePassword)
        {
            throw new ArgumentException("Invalid event type. Expected UserRequestedChangePassword.", nameof(eventMessage));
        }
        var payload = JsonDocument.Parse(JsonSerializer.Serialize(userRequestedChangePassword));
        return new OutboxEntry(
            Guid.CreateVersion7(),
            nameof(UserRequestedChangePassword),
            payload,
            DateTimeOffset.UtcNow,
            0
        );
    }
}
