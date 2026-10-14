
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StoErp.Modules.Auth.Domain.RefreshToken;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Conversions;

public sealed class RefreshTokenHashConversion : ValueConverter<RefreshTokenHash, string>
{
    public RefreshTokenHashConversion() : base(
        refreshTokenHash => refreshTokenHash.Value,
        str => RefreshTokenHash.From(str))
    {
    }


}
