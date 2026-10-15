using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StoErp.Modules.ProfileModule.Domain.ProfileEntity;

namespace StoErp.Modules.ProfileModule.Infrastructure.Persistence.Conversions;

public sealed class ProfileIdConversion : ValueConverter<ProfileId,Guid>
{
    public ProfileIdConversion() : base(
        profileId => profileId.Value,
        value => ProfileId.From(value))
    {
    }

}
