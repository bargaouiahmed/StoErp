using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoErp.Modules.ProfileModule.Domain.ProfileEntity;
using StoErp.Modules.ProfileModule.Infrastructure.Persistence.Conversions;

namespace StoErp.Modules.ProfileModule.Infrastructure.Persistence.Configurations;

public sealed class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.ToTable("profiles");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(new ProfileIdConversion())
            .ValueGeneratedNever();
        builder.Property(p => p.UserId)
            .HasConversion(new UserIdConversion())
            .IsRequired();
        builder.Property(p => p.Firstname)
            .HasConversion(new FirstnameConversion())
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(p => p.Lastname)
            .HasConversion(new LastnameConversion())
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(p=>p.LanguagePreference)
            .HasConversion(new LanguagePreferenceConversion())
            .HasMaxLength(2)
            .IsRequired();
        builder.HasIndex(p => p.UserId)
            .IsUnique();

    }
}
