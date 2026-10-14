


using StoErp.Shared.Application.Events;

namespace StoErp.Shared.Contracts.AuthModuleContracts;

public sealed record UserRequestedChangePassword(
    string Email,
    string PasswordResetTokenSecret
):IEvent;