

namespace StoErp.Modules.ProfileModule.Application.Persistence;

public interface IUnitOfWork
{

    Task CommitAsync(CancellationToken cancellationToken = default);
}
