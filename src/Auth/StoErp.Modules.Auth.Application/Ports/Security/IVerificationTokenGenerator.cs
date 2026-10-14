
using StoErp.Modules.Auth.Application.Commands.RegisterUser;

namespace StoErp.Modules.Auth.Application.Ports.Security;

public interface IVerificationTokenGenerator
{

    VerificationTokenResult GenerateVerificationToken(VerificationTokenOptions options, CancellationToken cancellationToken = default);
}
