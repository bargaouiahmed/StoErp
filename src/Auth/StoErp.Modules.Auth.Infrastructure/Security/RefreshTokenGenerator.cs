
using StoErp.Modules.Auth.Application.Ports.Security;
using StoErp.Modules.Auth.Domain.RefreshToken;

namespace StoErp.Modules.Auth.Infrastructure.Security;

public sealed class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public RefreshTokenSecret Generate()
    {
        return RefreshTokenSecret
        .From
        (Convert
        .ToBase64String(
            Guid
            .NewGuid()
            .ToByteArray())
            .Replace("=", "")
            .Replace("+", ""));
    }

}
