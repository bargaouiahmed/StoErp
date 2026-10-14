
using StoErp.Modules.Auth.Domain.RefreshToken;
using StoErp.Modules.Auth.Domain.User;

namespace StoErp.Modules.Auth.Application.Ports.Security;

public interface IRefreshTokenGenerator
{
    RefreshTokenSecret Generate();

}
