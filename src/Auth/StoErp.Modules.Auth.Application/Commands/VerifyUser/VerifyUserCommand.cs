using Mediator;

namespace StoErp.Modules.Auth.Application.Commands.VerifyUser;

public sealed record VerifyUserCommand(
    string Email,
    string TokenSecret
) : ICommand<Unit>;
