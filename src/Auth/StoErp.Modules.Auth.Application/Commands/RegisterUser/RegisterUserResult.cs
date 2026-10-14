namespace StoErp.Modules.Auth.Application.Commands.RegisterUser;

public sealed record RegisterUserResult
(
    Guid UserId,
    string Username,
    string Email
);
