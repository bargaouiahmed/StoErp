using System;
using StoErp.Modules.Auth.Domain.PasswordResetToken;
using StoErp.Modules.Auth.Domain.User;

namespace StoErp.Modules.Auth.Application.Ports.Persistence;

public interface IPasswordResetTokenReadRepository
{
    Task<PasswordResetToken?> GetByUserIdForUpdateAsync(UserId userId, CancellationToken cancellationToken);

}
