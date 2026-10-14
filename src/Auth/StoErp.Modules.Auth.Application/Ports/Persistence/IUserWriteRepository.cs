
using StoErp.Modules.Auth.Domain.User;

namespace StoErp.Modules.Auth.Application.Ports.Persistence;

public interface IUserWriteRepository
{

    Task<bool> ExistsByUsernameAsync(Username username, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken = default);

    void Add(User user);
}
