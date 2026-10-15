
using StoErp.Shared.Application.Events;
using StoErp.Shared.Contracts.AuthModuleContracts;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Outbox.Reader;

public class UserRequestedPasswordResetOutboxReaderStrategy : IOutboxEventReaderStrategy
{
    public bool CanHandle(string eventType)
    {
        return eventType == nameof(UserRequestedChangePassword);
    }

    public IEvent DeserializeEvent(OutboxEntry outboxEntry)
    {
        if(outboxEntry.EventType != nameof(UserRequestedChangePassword))
        {
            throw new ArgumentException($"Invalid event type. Expected {nameof(UserRequestedChangePassword)}.", nameof(outboxEntry));
        }
        if(outboxEntry.Payload == null)
        {
            throw new ArgumentException("Payload cannot be null.", nameof(outboxEntry));
        }
        
        if(!outboxEntry.Payload.RootElement.TryGetProperty("Email", out var emailProperty) || emailProperty.ValueKind != System.Text.Json.JsonValueKind.String)
        {
            throw new ArgumentException("Payload does not contain a valid 'Email' property.", nameof(outboxEntry));
        }
        if(!outboxEntry.Payload.RootElement.TryGetProperty("PasswordResetTokenSecret", out var passwordResetTokenProperty) || passwordResetTokenProperty.ValueKind != System.Text.Json.JsonValueKind.String)
        {
            throw new ArgumentException("Payload does not contain a valid 'PasswordResetTokenSecret' property.", nameof(outboxEntry));
        }

        if(!outboxEntry.Payload.RootElement.TryGetProperty("Username", out var usernameProperty) || usernameProperty.ValueKind != System.Text.Json.JsonValueKind.String)
        {
            throw new ArgumentException("Payload does not contain a valid 'Username' property.", nameof(outboxEntry));
        }
        if(!outboxEntry.Payload.RootElement.TryGetProperty("LanguagePreference", out var languagePreferenceProperty) || languagePreferenceProperty.ValueKind != System.Text.Json.JsonValueKind.String)
        {
            throw new ArgumentException("Payload does not contain a valid 'LanguagePreference' property.", nameof(outboxEntry));
        }
        return new UserRequestedChangePassword(
            emailProperty.GetString()!,
            passwordResetTokenProperty.GetString()!,
            usernameProperty.GetString()!,
            languagePreferenceProperty.GetString()!

        );
        
    }
}

