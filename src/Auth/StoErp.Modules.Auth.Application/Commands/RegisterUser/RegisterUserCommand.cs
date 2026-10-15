
using Mediator;
namespace StoErp.Modules.Auth.Application.Commands.RegisterUser;

public sealed record RegisterUserCommand : ICommand<RegisterUserResult>
{
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string Email { get; init; }
    public string LanguagePreference { get; init; } = "en";
    public required string Firstname { get; init; } 
    public required string Lastname { get; init; } 
    public required DateTimeOffset DateOfBirth { get; init; } 

}
