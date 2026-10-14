using System;
using StoErp.Modules.Auth.Domain.PasswordResetToken;

namespace StoErp.Modules.Auth.Application.Ports.Security;

public interface IPasswordResetTokenHasher
{
    string Hash(string tokenSecret);
    bool Verify(string tokenSecret, PasswordResetTokenHash hash);

}
