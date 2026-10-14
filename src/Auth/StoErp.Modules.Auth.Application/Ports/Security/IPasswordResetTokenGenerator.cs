using System;
using StoErp.Modules.Auth.Application.Commands.RequestPasswordReset;

namespace StoErp.Modules.Auth.Application.Ports.Security;

public interface IPasswordResetTokenGenerator
{
    string Generate(PasswordResetTokenOptions options);

}
