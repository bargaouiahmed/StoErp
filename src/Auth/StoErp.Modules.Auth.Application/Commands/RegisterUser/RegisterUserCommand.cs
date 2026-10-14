
using Mediator;
namespace StoErp.Modules.Auth.Application.Commands.RegisterUser;

public sealed record RegisterUserCommand : ICommand<RegisterUserResult>
{
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string Email { get; init; }

}
