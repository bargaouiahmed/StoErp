
using System.Security.Cryptography;
using StoErp.Modules.Auth.Application.Commands.RegisterUser;
using StoErp.Modules.Auth.Application.Ports.Security;

namespace StoErp.Modules.Auth.Infrastructure.Security;

public class VerificationTokenGenerator : IVerificationTokenGenerator
{
    public VerificationTokenResult GenerateVerificationToken(VerificationTokenOptions options, CancellationToken cancellationToken = default)
    {
        var tokenSecret = GenerateTokenSecret(options.Length, options.OnlyNumbers);
        var expiresAt = DateTimeOffset.UtcNow.Add(options.ValidityPeriod);

        return new VerificationTokenResult(tokenSecret, expiresAt);

    }

    private string GenerateTokenSecret(int length, bool onlyNumbers)
    {
        var chars = onlyNumbers switch 
        {
            true => "0123456789",
            false => "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"
        };  
        var tokenSecret = RandomNumberGenerator.GetString(chars, length);
        return tokenSecret;
        

            
    }
}
