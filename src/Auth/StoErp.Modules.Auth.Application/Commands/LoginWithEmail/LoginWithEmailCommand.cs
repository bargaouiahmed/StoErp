using Mediator;
using StoErp.Modules.Auth.Application.Commands.Login;

namespace StoErp.Modules.Auth.Application.Commands.LoginWithEmail;

public record class LoginWithEmailCommand(string Email, string Password) : ICommand<LoginResult>;
