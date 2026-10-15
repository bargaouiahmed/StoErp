

namespace StoErp.Modules.Auth.Infrastructure.Security;

public sealed class JwtOptions
{ 
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string Secret { get; init; }

}
