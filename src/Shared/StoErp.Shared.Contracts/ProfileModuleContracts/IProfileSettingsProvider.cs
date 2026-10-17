

namespace StoErp.Shared.Contracts.ProfileModuleContracts;

public interface IProfileSettingsProvider
{

    Task<LanguagePreferenceSettings> GetLanguageSettingsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Profile?> GetProfileByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
