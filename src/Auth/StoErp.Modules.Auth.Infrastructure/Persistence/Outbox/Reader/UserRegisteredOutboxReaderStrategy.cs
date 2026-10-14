
using StoErp.Shared.Application.Events;
using StoErp.Shared.Contracts.AuthModuleContracts;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Outbox.Reader;

public sealed class UserRegisteredOutboxReaderStrategy : IOutboxEventReaderStrategy
{
    public bool CanHandle(string eventType)
    {
        return eventType == nameof(UserRegisteredEvent);
    }

    public IEvent DeserializeEvent(OutboxEntry outboxMessage)
    {
        if (outboxMessage.EventType != nameof(UserRegisteredEvent))
        {
            throw new ArgumentException($"Invalid event type. Expected {nameof(UserRegisteredEvent)}.", nameof(outboxMessage));
        }

        if (outboxMessage.Payload == null)
        {
            throw new ArgumentException("Payload cannot be null.", nameof(outboxMessage));
        }
        if (!outboxMessage.Payload.RootElement.TryGetProperty("UserId", out var userIdProperty) || userIdProperty.ValueKind != System.Text.Json.JsonValueKind.String)
        {
            throw new ArgumentException("Payload does not contain a valid 'UserId' property.", nameof(outboxMessage));
        }
        if (!outboxMessage.Payload.RootElement.TryGetProperty("Email", out var emailProperty) || emailProperty.ValueKind != System.Text.Json.JsonValueKind.String)
        {
            throw new ArgumentException("Payload does not contain a valid 'Email' property.", nameof(outboxMessage));
        }
        if (!outboxMessage.Payload.RootElement.TryGetProperty("Username", out var usernameProperty) || usernameProperty.ValueKind != System.Text.Json.JsonValueKind.String)
        {
            throw new ArgumentException("Payload does not contain a valid 'Username' property.", nameof(outboxMessage));
        }
        if (!outboxMessage.Payload.RootElement.TryGetProperty("RegisteredAt", out var registeredAtProperty) || registeredAtProperty.ValueKind != System.Text.Json.JsonValueKind.String)
        {
            throw new ArgumentException("Payload does not contain a valid 'RegisteredAt' property.", nameof(outboxMessage));
        }

        return new UserRegisteredEvent(
            Guid.Parse(userIdProperty.GetString()!),
            usernameProperty.GetString()!,
            emailProperty.GetString()!,
            DateTimeOffset.Parse(registeredAtProperty.GetString()!)
        );

    }
}
