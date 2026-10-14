
using StoErp.Modules.Auth.Application.Commands.Login;
using StoErp.Modules.Auth.Application.Ports.Persistence.Projections;

namespace StoErp.Modules.Auth.Application.Ports.Security;

public interface IAccessTokenGenerator
{
    AccessTokenResult GenerateAccessToken(UserLoginData userLoginData, TimeSpan timeBeforeExpiration, CancellationToken cancellationToken = default);
}
