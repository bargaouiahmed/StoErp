using System;
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
        var random = new Random();
        var tokenSecret = new char[length];

        for (int i = 0; i < length; i++)
        {
            if (onlyNumbers)
            {
                tokenSecret[i] = (char)('0' + random.Next(0, 10));
            }
            else
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                tokenSecret[i] = chars[random.Next(chars.Length)];
            }
        }

        return new string(tokenSecret);
    }
}
