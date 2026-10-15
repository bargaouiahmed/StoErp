
using Mediator;

namespace StoErp.Modules.Auth.Application.Commands.RequestPasswordReset;

public record RequestPasswordResetCommand(string Email) : ICommand<Unit>;

