
using Microsoft.AspNetCore.Identity;
using StoErp.Modules.Auth.Application.Ports.Security;
using StoErp.Modules.Auth.Domain.User;

namespace StoErp.Modules.Auth.Infrastructure.Security;

public sealed class PasswordHasher : IPasswordHasher
{
    private readonly IPasswordHasher<object> _passwordHasher;
    private readonly object _dummy = new object();
    public PasswordHasher()
    {
        _passwordHasher = new PasswordHasher<object>();
    }

    public PasswordHash Hash(string password)
    {
        var hashedPassword = _passwordHasher.HashPassword(_dummy, password);
        return PasswordHash.From(hashedPassword);
    }

    public bool Verify(string password, PasswordHash passwordHash)

    {
        var result = _passwordHasher.VerifyHashedPassword(_dummy, passwordHash.Value, password);
        return result == PasswordVerificationResult.Success;
    }

}
