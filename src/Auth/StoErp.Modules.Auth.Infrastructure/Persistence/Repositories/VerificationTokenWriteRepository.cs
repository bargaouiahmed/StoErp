
using StoErp.Modules.Auth.Application.Ports.Persistence;
using StoErp.Modules.Auth.Domain.VerificationToken;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Repositories;

public sealed class VerificationTokenWriteRepository : IVerificationTokenWriteRepository
{

    private readonly AuthDbContext _dbContext;
    public VerificationTokenWriteRepository(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public void Add(VerificationToken verificationToken)
    {
        _dbContext.VerificationTokens.Add(verificationToken);
    }


}
