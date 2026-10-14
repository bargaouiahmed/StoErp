
using Microsoft.EntityFrameworkCore;
using StoErp.Modules.Auth.Application.Ports.Persistence;
using StoErp.Modules.Auth.Domain.User;
using StoErp.Modules.Auth.Domain.VerificationToken;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Repositories;

public sealed class VerificationTokenReadRepository : IVerificationTokenReadRepository
{
    private readonly AuthDbContext _dbContext;

    public VerificationTokenReadRepository(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<VerificationToken?> GetByUserIdForUpdateAsync(UserId userId, CancellationToken cancellationToken)
    {
        return await _dbContext.VerificationTokens
            .Where(vt => vt.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);

    }
}
