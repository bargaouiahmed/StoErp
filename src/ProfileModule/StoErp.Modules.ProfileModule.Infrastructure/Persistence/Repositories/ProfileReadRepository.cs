using System;
using Microsoft.EntityFrameworkCore;
using StoErp.Modules.ProfileModule.Application.Persistence.Repositories;
using StoErp.Modules.ProfileModule.Domain.ProfileEntity;
using StoErp.Shared.Contracts.ProfileModuleContracts;

namespace StoErp.Modules.ProfileModule.Infrastructure.Persistence.Repositories;

public class ProfileReadRepository : IProfileReadRepository
{
    private readonly ProfileDbContext _dbContext;
    public ProfileReadRepository(ProfileDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Profile?> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Profiles.AsNoTracking().FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
        
    }

    public Task<LanguagePreferenceSettings?> GetLanguagePreferenceSettingsByUserIdAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Profiles.AsNoTracking()
            .Where(p => p.UserId == userId)
            .Select(p => new LanguagePreferenceSettings(p.LanguagePreference.Value))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
