
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StoErp.Modules.Auth.Domain.RefreshToken;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Conversions;

public sealed class RefreshTokenIdConversion : ValueConverter<RefreshTokenId, Guid>
{
    public RefreshTokenIdConversion() : base(
        refreshTokenId => refreshTokenId.Value,
        guid => RefreshTokenId.From(guid))
    {
    }

}
