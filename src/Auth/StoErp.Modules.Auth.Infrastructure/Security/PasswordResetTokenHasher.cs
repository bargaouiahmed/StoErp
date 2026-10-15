
using Microsoft.AspNetCore.Identity;
using StoErp.Modules.Auth.Application.Ports.Security;
using StoErp.Modules.Auth.Domain.PasswordResetToken;

namespace StoErp.Modules.Auth.Infrastructure.Security;

public class PasswordResetTokenHasher : IPasswordResetTokenHasher
{   
    private readonly PasswordHasher<object> _passwordHasher;
    private readonly object dummyObject = new object();
    public PasswordResetTokenHasher()
    {
        _passwordHasher = new PasswordHasher<object>();
    }
    public PasswordResetTokenHash Hash(string tokenSecret)
    {
        var hashedToken = _passwordHasher.HashPassword(dummyObject, tokenSecret);
        return PasswordResetTokenHash.From(hashedToken);
        
    }

    public bool Verify(string tokenSecret, PasswordResetTokenHash hash)
    {
        var result = _passwordHasher.VerifyHashedPassword(dummyObject, hash.Value, tokenSecret);
        return result == PasswordVerificationResult.Success;
    }
}
