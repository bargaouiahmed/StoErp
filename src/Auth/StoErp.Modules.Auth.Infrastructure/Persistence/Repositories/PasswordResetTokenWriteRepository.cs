
using StoErp.Modules.Auth.Application.Ports.Persistence;
using StoErp.Modules.Auth.Domain.PasswordResetToken;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Repositories;

public class PasswordResetTokenWriteRepository : IPasswordResetTokenWriteRepository
{
    private readonly AuthDbContext _dbContext;
    public PasswordResetTokenWriteRepository(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public void Add(PasswordResetToken passwordResetToken)
    {
        _dbContext.PasswordResetTokens.Add(passwordResetToken);
    }
}
