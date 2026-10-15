using StoErp.Modules.ProfileModule.Domain.ProfileEntity;
using StoErp.Shared.Contracts.ProfileModuleContracts;

namespace StoErp.Modules.ProfileModule.Application.Persistence.Repositories;
public interface IProfileReadRepository
{
    public Task<Profile?> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken = default); 
    public Task<LanguagePreferenceSettings?> GetLanguagePreferenceSettingsByUserIdAsync(UserId userId, CancellationToken cancellationToken = default);

}
