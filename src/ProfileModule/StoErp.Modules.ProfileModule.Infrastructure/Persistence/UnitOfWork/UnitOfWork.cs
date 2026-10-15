using System;
using StoErp.Modules.ProfileModule.Application.Persistence;

namespace StoErp.Modules.ProfileModule.Infrastructure.Persistence.UnitOfWork;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ProfileDbContext _dbContext;

    public UnitOfWork(ProfileDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
         await _dbContext.SaveChangesAsync(cancellationToken);
    }

}
