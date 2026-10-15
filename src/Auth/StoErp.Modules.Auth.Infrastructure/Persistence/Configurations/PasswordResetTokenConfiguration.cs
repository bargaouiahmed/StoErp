
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoErp.Modules.Auth.Domain.PasswordResetToken;
using StoErp.Modules.Auth.Infrastructure.Persistence.Conversions;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Configurations;

public class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
{
    public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
    {
        builder.ToTable("password_reset_tokens");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .HasConversion(new PasswordResetTokenIdConversion())
            .ValueGeneratedNever();
        builder.Property(t => t.TokenHash)
            .HasConversion(new PasswordResetTokenHashConversion())
            .IsRequired();
        builder.Property(t=>t.UserId)
        .HasConversion(new UserIdConversion())
        .IsRequired();
        builder.HasOne(t=>t.User)
            .WithMany(u=>u.PasswordResetTokens)
            .HasForeignKey(t=>t.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Property(t => t.CreatedAt)
            .IsRequired();
        builder.Property(t => t.ExpiresAt)
            .IsRequired();
        builder.HasIndex(t => t.UserId)
            .IsUnique();
        builder.Property(t=>t.TokenHash)
        .HasMaxLength(255)
        .IsRequired();
        
        
    }
}
