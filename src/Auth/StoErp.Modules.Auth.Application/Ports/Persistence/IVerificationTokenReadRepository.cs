
using StoErp.Modules.Auth.Application.Ports.Persistence.Projections;
using StoErp.Modules.Auth.Domain.User;
using StoErp.Modules.Auth.Domain.VerificationToken;

namespace StoErp.Modules.Auth.Application.Ports.Persistence;

public interface IVerificationTokenReadRepository
{
    Task<VerificationToken?> GetByUserIdForUpdateAsync(UserId userId, CancellationToken cancellationToken);
}
