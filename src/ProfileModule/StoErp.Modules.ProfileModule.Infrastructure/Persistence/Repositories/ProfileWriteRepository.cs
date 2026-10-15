
using StoErp.Modules.ProfileModule.Application.Persistence.Repositories;
using StoErp.Modules.ProfileModule.Domain.ProfileEntity;
using Microsoft.EntityFrameworkCore;
namespace StoErp.Modules.ProfileModule.Infrastructure.Persistence.Repositories;

public class ProfileWriteRepository : IProfileWriteRepository
{   
    private readonly ProfileDbContext _dbContext;
    public ProfileWriteRepository(ProfileDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public void Add(Profile profile)
    {
        _dbContext.Profiles.Add(profile);
    }

    public async Task<bool> ExistsByUserIdAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Profiles.AnyAsync(p => p.UserId == userId, cancellationToken);
    }
}
