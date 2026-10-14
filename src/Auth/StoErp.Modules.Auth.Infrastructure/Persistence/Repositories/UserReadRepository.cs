
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoErp.Modules.Auth.Application.Ports.Persistence;
using StoErp.Modules.Auth.Application.Ports.Persistence.Projections;
using StoErp.Modules.Auth.Domain.User;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Repositories;

public sealed class UserReadRepository : IUserReadRepository
{
    private readonly AuthDbContext _dbContext;
    public UserReadRepository(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<UserLoginData?> GetLoginDataByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        var userEntity = await _dbContext
        .Users
        .AsNoTracking()
        .Where(u => u.Email == email)
        .Select(u => new
        {
            u.Id,
            u.Username,
            u.Email,
            u.PasswordHash,
            u.Verified,
            u.CreatedAt,
            u.UpdatedAt,
            u.DeletedAt
        }
            ).FirstOrDefaultAsync(cancellationToken);
        if (userEntity is null)
        {
            return null;
        }
        return new UserLoginData(userEntity.Id, userEntity.Username, userEntity.Email, userEntity.PasswordHash, userEntity.Verified, userEntity.CreatedAt, userEntity.UpdatedAt, userEntity.DeletedAt);
    }

    public async Task<UserLoginData?> GetLoginDataByUsernameAsync(Username username, CancellationToken cancellationToken = default)
    {
        var userEntity = await _dbContext
        .Users
        .AsNoTracking()
        .Where(u => u.Username == username)
        .Select(u => new
        {
            u.Id,
            u.Username,
            u.Email,
            u.PasswordHash,
            u.Verified,
            u.CreatedAt,
            u.UpdatedAt,
            u.DeletedAt
        }
            ).FirstOrDefaultAsync(cancellationToken);
        if (userEntity is null)
        {
            return null;
        }
        return new UserLoginData(userEntity.Id, userEntity.Username, userEntity.Email, userEntity.PasswordHash, userEntity.Verified, userEntity.CreatedAt, userEntity.UpdatedAt, userEntity.DeletedAt);


    }
    public async Task<User?> GetByEmailForUpdateAsync(Email email, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        return user;
    }
}
