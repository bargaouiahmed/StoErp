
using Mediator;

namespace StoErp.Modules.Auth.Application.Commands.ResetPassword;

public sealed record ResetPasswordCommand(
    string Email,
    string NewPassword,
    string ResetTokenSecret
):ICommand<Unit>;
