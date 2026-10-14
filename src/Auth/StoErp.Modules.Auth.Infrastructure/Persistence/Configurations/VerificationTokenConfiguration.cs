
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoErp.Modules.Auth.Domain.VerificationToken;
using StoErp.Modules.Auth.Infrastructure.Persistence.Conversions;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Configurations;

public sealed class VerificationTokenConfiguration : IEntityTypeConfiguration<VerificationToken>
{
    public void Configure(EntityTypeBuilder<VerificationToken> builder)
    {
        builder.ToTable("verification_tokens");

        builder.HasKey(token => token.Id);
        builder.Property(token => token.Id)
            .HasConversion(new VerificationTokenIdConversion())
            .ValueGeneratedNever();
        builder.Property(token => token.TokenHash)
            .HasConversion(new VerificationTokenHashConversion())
            .IsRequired();
        builder.Property(token => token.UserId)
            .HasConversion(new UserIdConversion())
            .IsRequired();

        builder.HasIndex(token => token.UserId)
            .IsUnique();
        builder.HasOne(token => token.User)
            .WithMany(u => u.VerificationTokens)
            .HasForeignKey(token => token.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}