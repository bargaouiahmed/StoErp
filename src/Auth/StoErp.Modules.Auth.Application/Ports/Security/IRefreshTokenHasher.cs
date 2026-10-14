
using StoErp.Modules.Auth.Domain.RefreshToken;

namespace StoErp.Modules.Auth.Application.Ports.Security;

public interface IRefreshTokenHasher
{
    RefreshTokenHash Hash(string refreshTokenSecret);
    bool Verify(string refreshTokenSecret, RefreshTokenHash refreshTokenHash);
}
