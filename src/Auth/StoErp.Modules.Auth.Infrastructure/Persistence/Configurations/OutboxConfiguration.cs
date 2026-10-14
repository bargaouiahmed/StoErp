using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoErp.Shared.Infrastructure.Messaging.Outbox;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Configurations;

public sealed class OutboxConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        {
            builder.ToTable(
                "outbox_messages");

            builder.HasKey(message => message.Id);

            builder.Property(message => message.EventType)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(message => message.Payload)
                .HasColumnType("jsonb")
                .IsRequired();

            builder.Property(message => message.OccurredAt)
                .IsRequired();

            builder.Property(message => message.AttemptCount)
                .IsRequired();

            builder.Property(message => message.ProcessedAt);

            builder.Property(message => message.NextAttemptAt);

            builder.Property(message => message.DeadAt);

            builder.Property(message => message.ErrorHistory)
                .HasColumnType("jsonb");

            builder.HasIndex(message => new
            {
                message.ProcessedAt,
                message.NextAttemptAt
            });
        }
    }

}
