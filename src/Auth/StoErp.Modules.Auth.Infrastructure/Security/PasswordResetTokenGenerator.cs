
using System.Security.Cryptography;
using StoErp.Modules.Auth.Application.Commands.RequestPasswordReset;
using StoErp.Modules.Auth.Application.Ports.Security;

namespace StoErp.Modules.Auth.Infrastructure.Security;

public class PasswordResetTokenGenerator : IPasswordResetTokenGenerator
{
    public string Generate(PasswordResetTokenOptions options)
    {
        var onlyNumbers = options.OnlyNumbers;
        var tokenSecretLength = options.TokenLength;

        var chars = onlyNumbers ? "0123456789" : "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return RandomNumberGenerator.GetString(chars, tokenSecretLength);
        
    }

}
