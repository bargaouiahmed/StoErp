

namespace StoErp.Shared.Contracts.ProfileModuleContracts;

public interface IProfileSettingsProvider
{

    Task<LanguagePreferenceSettings> GetLanguageSettingsAsync(Guid userId, CancellationToken cancellationToken = default);
}
