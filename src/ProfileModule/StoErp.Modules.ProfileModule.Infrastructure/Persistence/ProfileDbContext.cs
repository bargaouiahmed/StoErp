using System;
using Microsoft.EntityFrameworkCore;
using StoErp.Modules.ProfileModule.Domain.ProfileEntity;

namespace StoErp.Modules.ProfileModule.Infrastructure.Persistence;

public sealed class ProfileDbContext(DbContextOptions<ProfileDbContext> options) : DbContext(options)
{

public DbSet<Profile> Profiles => Set<Profile>();

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    modelBuilder.HasDefaultSchema("profile");
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProfileDbContext).Assembly);
}
}
