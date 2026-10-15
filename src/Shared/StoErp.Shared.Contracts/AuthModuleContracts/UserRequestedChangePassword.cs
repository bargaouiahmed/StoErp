


using StoErp.Shared.Application.Events;

namespace StoErp.Shared.Contracts.AuthModuleContracts;

public sealed record UserRequestedChangePassword(
    string Email,
    string PasswordResetTokenSecret,
    string Username,
    string LanguagePreference="en"
):IEvent;