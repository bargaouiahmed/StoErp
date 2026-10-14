
using StoErp.Modules.Auth.Application.Ports.Persistence;
using StoErp.Modules.Auth.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Repositories;

public sealed class UserWriteRepository : IUserWriteRepository
{
    private readonly AuthDbContext _dbContext;
    public UserWriteRepository(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public Task<bool> ExistsByUsernameAsync(Username username, CancellationToken cancellationToken = default)
    {
        return _dbContext.Users.AnyAsync(user => user.Username == username, cancellationToken);
    }
    public Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return _dbContext.Users.AnyAsync(user => user.Email == email, cancellationToken);
    }

    public void Add(User user)
    {
        _dbContext.Users.Add(user);
    }
}
