
using StoErp.Modules.ProfileModule.Application.Persistence.Repositories;
using StoErp.Modules.ProfileModule.Domain.ProfileEntity;
using StoErp.Shared.Contracts.ProfileModuleContracts;

namespace StoErp.Modules.ProfileModule.Application.Ports;

public class ProfileSettingsProvider : IProfileSettingsProvider
{
    private readonly IProfileReadRepository _profileReadRepository;

    public ProfileSettingsProvider(IProfileReadRepository profileReadRepository)
    {
        _profileReadRepository = profileReadRepository;
    }
    

    public async Task<LanguagePreferenceSettings> GetLanguageSettingsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userIdValue = UserId.From(userId);
        var languagePreferenceSettings = await _profileReadRepository.GetLanguagePreferenceSettingsByUserIdAsync(userIdValue, cancellationToken);

        return languagePreferenceSettings ?? new LanguagePreferenceSettings("en");

        
    }

    public async Task<Shared.Contracts.ProfileModuleContracts.Profile?> GetProfileByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userIdValue = UserId.From(userId);
        return await _profileReadRepository.GetByUserIdAsync(userIdValue, cancellationToken);
    }
}
