using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoErp.Modules.Auth.Domain.RefreshToken;
using StoErp.Modules.Auth.Domain.User;
using StoErp.Modules.Auth.Infrastructure.Persistence.Conversions;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Configurations;

public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{

    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {

        builder.ToTable("refresh_tokens");
        builder.HasKey(refreshToken => refreshToken.Id);
        builder.Property(refreshToken => refreshToken.Id)
            .HasConversion(new RefreshTokenIdConversion())
            .ValueGeneratedNever();
        builder.Property(refreshToken => refreshToken.UserId)
            .HasConversion(new UserIdConversion())
            .IsRequired();


        builder.Property(refreshToken => refreshToken.Hash)
            .HasConversion(new RefreshTokenHashConversion())
            .HasMaxLength(500)
            .IsRequired();
        builder.Property(refreshToken => refreshToken.CreatedAt)
            .IsRequired();
        builder.Property(refreshToken => refreshToken.ExpiresAt)
            .IsRequired();
        builder.Property(refreshToken => refreshToken.RevokedAt);

        builder.HasIndex(refreshToken => refreshToken.Hash)
            .IsUnique();
        builder.HasIndex(refreshToken => refreshToken.UserId);
        builder.HasOne(refreshToken => refreshToken.User)
            .WithMany(user => user.RefreshTokens)
            .HasForeignKey(refreshToken => refreshToken.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
