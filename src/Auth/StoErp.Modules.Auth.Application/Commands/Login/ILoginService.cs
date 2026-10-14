
using StoErp.Modules.Auth.Application.Ports.Persistence.Projections;
using StoErp.Modules.Auth.Domain.User;

namespace StoErp.Modules.Auth.Application.Commands.Login;

public interface ILoginService
{
    Task<LoginResult> LoginAsync(UserLoginData user, string password, CancellationToken cancellationToken = default);
}
