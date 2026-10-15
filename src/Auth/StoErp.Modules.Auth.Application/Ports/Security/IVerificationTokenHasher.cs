using StoErp.Modules.Auth.Domain.VerificationToken;

namespace StoErp.Modules.Auth.Application.Ports.Security;

public interface IVerificationTokenHasher
{
    VerificationTokenHash Hash(string tokenSecret);
    bool Verify(string tokenSecret, VerificationTokenHash hash);
}
