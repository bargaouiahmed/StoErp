

namespace StoErp.Modules.Auth.Application.Ports.Persistence;

public interface IUnitOfWork
{
    public Task CommitAsync(CancellationToken cancellationToken = default);
}
