
using Microsoft.AspNetCore.Identity;
using StoErp.Modules.Auth.Application.Ports.Security;
using StoErp.Modules.Auth.Domain.VerificationToken;

namespace StoErp.Modules.Auth.Infrastructure.Security;

public class VerificationTokenHasher : IVerificationTokenHasher
{
    private readonly PasswordHasher<object> _passwordHasher;
    private readonly object dummy = new();
    public VerificationTokenHasher()
    {
        _passwordHasher = new PasswordHasher<object>();
    }
    public VerificationTokenHash Hash(string tokenSecret)
    {
        var hashedToken = _passwordHasher.HashPassword(dummy, tokenSecret);
        return VerificationTokenHash.From(hashedToken);
    }

    public bool Verify(string tokenSecret, VerificationTokenHash hash)
    {
        var result = _passwordHasher.VerifyHashedPassword(dummy, hash.Value, tokenSecret);
        return result == PasswordVerificationResult.Success;
    }
}
