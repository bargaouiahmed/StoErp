
using StoErp.Modules.ProfileModule.Domain.ProfileEntity;
namespace StoErp.Modules.ProfileModule.Application.Persistence.Repositories;

public interface IProfileWriteRepository
{
    public void Add(Profile profile);
    public Task<bool> ExistsByUserIdAsync(UserId userId, CancellationToken cancellationToken = default);
}
