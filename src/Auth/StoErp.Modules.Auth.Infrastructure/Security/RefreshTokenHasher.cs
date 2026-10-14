using Microsoft.AspNetCore.Identity;
using StoErp.Modules.Auth.Application.Ports.Security;
using StoErp.Modules.Auth.Domain.RefreshToken;

namespace StoErp.Modules.Auth.Infrastructure.Security;

public sealed class RefreshTokenHasher : IRefreshTokenHasher
{
    private readonly PasswordHasher<object> _passwordHasher;
    private readonly object _dummy = new object();
    public RefreshTokenHasher()
    {
        _passwordHasher = new PasswordHasher<object>();
    }
    public RefreshTokenHash Hash(string refreshTokenSecret)
    {
        var hashedRefreshToken = _passwordHasher.HashPassword(_dummy, refreshTokenSecret);
        return RefreshTokenHash.From(hashedRefreshToken);
    }

    public bool Verify(string refreshTokenSecret, RefreshTokenHash refreshTokenHash)
    {
        var result = _passwordHasher.VerifyHashedPassword(_dummy, refreshTokenHash.Value, refreshTokenSecret);
        return result == PasswordVerificationResult.Success;
    }
}
