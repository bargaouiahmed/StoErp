
using Mediator;
using StoErp.Modules.Auth.Application.Commands.Login;
using StoErp.Modules.Auth.Application.Ports.Persistence;
using StoErp.Modules.Auth.Domain.User;

namespace StoErp.Modules.Auth.Application.Commands.LoginWithEmail;

public sealed class LoginWithEmailCommandHandler : ICommandHandler<LoginWithEmailCommand, LoginResult>
{
    private readonly IUserReadRepository _userReadRepository;
    private readonly ILoginService _loginService;
    public LoginWithEmailCommandHandler(IUserReadRepository userReadRepository, ILoginService loginService)
    {
        _userReadRepository = userReadRepository;
        _loginService = loginService;
    }
    public async ValueTask<LoginResult> Handle(LoginWithEmailCommand command, CancellationToken cancellationToken)
    {
        var email = Email.From(command.Email);
        var user = await _userReadRepository.GetLoginDataByEmailAsync(email, cancellationToken);
        if (user is null)
        {
            throw new InvalidOperationException("Invalid credentials..");
        }
        return await _loginService.LoginAsync(user, command.Password, cancellationToken);



    }
}
