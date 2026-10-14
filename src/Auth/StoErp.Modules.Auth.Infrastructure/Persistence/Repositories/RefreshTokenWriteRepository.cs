
using StoErp.Modules.Auth.Application.Ports.Persistence;
using StoErp.Modules.Auth.Domain.RefreshToken;
using StoErp.Modules.Auth.Domain.User;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Repositories;

public sealed class RefreshTokenWriteRepository : IRefreshTokenWriteRepository
{
    private readonly AuthDbContext _dbContext;
    public RefreshTokenWriteRepository(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public void AddRefreshTokenToUser(UserId userId, RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        var refreshTokenId = RefreshTokenId.From(refreshToken.Id.Value);
        var refreshTokenHash = RefreshTokenHash.From(refreshToken.Hash.Value);
        var refreshTokenEntity = RefreshToken.Create(refreshTokenId, userId, refreshTokenHash, refreshToken.CreatedAt, refreshToken.ExpiresAt);
        _dbContext.RefreshTokens.Add(refreshTokenEntity);

    }
}
