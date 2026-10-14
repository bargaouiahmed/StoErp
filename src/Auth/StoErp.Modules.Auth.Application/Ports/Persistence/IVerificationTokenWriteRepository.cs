
using StoErp.Modules.Auth.Domain.VerificationToken;

namespace StoErp.Modules.Auth.Application.Ports.Persistence;

public interface IVerificationTokenWriteRepository
{
    void Add(VerificationToken verificationToken);
}
