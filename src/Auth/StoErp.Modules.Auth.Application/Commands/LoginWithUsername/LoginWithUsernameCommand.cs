using Mediator;
using StoErp.Modules.Auth.Application.Commands.Login;

namespace StoErp.Modules.Auth.Application.Commands.LoginWithUsername;

public sealed record LoginWithUsernameCommand(string Username, string Password) : ICommand<LoginResult>;

