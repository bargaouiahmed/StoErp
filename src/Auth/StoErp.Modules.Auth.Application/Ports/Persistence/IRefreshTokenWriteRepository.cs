
using StoErp.Modules.Auth.Domain.RefreshToken;
using StoErp.Modules.Auth.Domain.User;

namespace StoErp.Modules.Auth.Application.Ports.Persistence;

public interface IRefreshTokenWriteRepository
{
    public void AddRefreshTokenToUser(UserId userId, RefreshToken refreshToken, CancellationToken cancellationToken);
}
