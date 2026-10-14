
using StoErp.Modules.Auth.Application.Ports.Persistence;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.UnitOfWork;

public sealed class AuthUnitOfWork : IUnitOfWork
{
    private readonly AuthDbContext _dbContext;
    public AuthUnitOfWork(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task CommitAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

}
