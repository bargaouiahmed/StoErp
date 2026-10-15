
using Microsoft.EntityFrameworkCore;
using StoErp.Modules.Auth.Application.Ports.Persistence;
using StoErp.Modules.Auth.Domain.PasswordResetToken;
using StoErp.Modules.Auth.Domain.User;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Repositories;

public class PasswordResetTokenReadRepository : IPasswordResetTokenReadRepository
{
    private readonly AuthDbContext _dbContext;
    public PasswordResetTokenReadRepository(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<PasswordResetToken?> GetByUserIdForUpdateAsync(UserId userId, CancellationToken cancellationToken)
    {
        var token = await _dbContext.PasswordResetTokens
            .Where(t => t.UserId == userId && !t.IsUsed && t.ExpiresAt > DateTimeOffset.UtcNow)
            .OrderByDescending(t => t.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);    
        return token;
    }
}
