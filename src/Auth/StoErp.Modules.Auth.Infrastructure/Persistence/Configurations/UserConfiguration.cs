using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoErp.Modules.Auth.Domain.User;
using StoErp.Modules.Auth.Infrastructure.Persistence.Conversions;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(user => user.Id);
        builder.Property(user => user.Id)
            .HasConversion(new UserIdConversion())
            .ValueGeneratedNever();
        builder.Property(user => user.Username)
            .HasConversion(new UsernameConversion())
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(user => user.Email)
            .HasConversion(new EmailConversion())
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(user => user.PasswordHash)
            .HasConversion(new PasswordHashConversion())
            .HasMaxLength(500)
            .IsRequired();


        builder.HasIndex(user => user.Username)
            .IsUnique();
        builder.HasIndex(user => user.Email)
            .IsUnique();






    }

}
