
using Mediator;
using StoErp.Modules.Auth.Application.Commands.Login;
using StoErp.Modules.Auth.Application.Ports.Persistence;
using StoErp.Modules.Auth.Domain.User;

namespace StoErp.Modules.Auth.Application.Commands.LoginWithUsername;

public sealed class LoginWithUsernameCommandHandler : ICommandHandler<LoginWithUsernameCommand, LoginResult>
{
    private readonly IUserReadRepository _userReadRepository;
    private readonly ILoginService _loginService;
    public LoginWithUsernameCommandHandler(IUserReadRepository userReadRepository, ILoginService loginService)
    {
        _userReadRepository = userReadRepository;
        _loginService = loginService;
    }
    public async ValueTask<LoginResult> Handle(LoginWithUsernameCommand command, CancellationToken cancellationToken)
    {
        var username = Username.From(command.Username);
        var user = await _userReadRepository.GetLoginDataByUsernameAsync(username, cancellationToken);
        if (user is null)
        {
            throw new InvalidOperationException("Invalid credentials..");
        }
        return await _loginService.LoginAsync(user, command.Password, cancellationToken);
    }
}
