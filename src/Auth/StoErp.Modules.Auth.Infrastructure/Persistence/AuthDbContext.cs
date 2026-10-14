
using Microsoft.EntityFrameworkCore;
using StoErp.Modules.Auth.Domain.RefreshToken;
using StoErp.Modules.Auth.Domain.User;
using StoErp.Modules.Auth.Domain.VerificationToken;
using StoErp.Shared.Infrastructure.Messaging.Outbox;

namespace StoErp.Modules.Auth.Infrastructure.Persistence;

public sealed class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }


    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<VerificationToken> VerificationTokens => Set<VerificationToken>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("auth");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);
    }
}
