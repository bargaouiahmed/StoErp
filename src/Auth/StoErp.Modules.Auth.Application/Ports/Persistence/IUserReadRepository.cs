
using StoErp.Modules.Auth.Application.Ports.Persistence.Projections;
using StoErp.Modules.Auth.Domain.User;

namespace StoErp.Modules.Auth.Application.Ports.Persistence;

public interface IUserReadRepository
{
    Task<UserLoginData?> GetLoginDataByUsernameAsync(Username username, CancellationToken cancellationToken = default);
    Task<UserLoginData?> GetLoginDataByEmailAsync(Email email, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailForUpdateAsync(Email email, CancellationToken cancellationToken = default);
    Task<UserVerificationData?> GetVerificationDataByIdAsync(UserId userId, CancellationToken cancellationToken = default);
    Task<UserPasswordResetData?> GetPasswordResetDataByEmailAsync(Email userEmail, CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken = default);
}
