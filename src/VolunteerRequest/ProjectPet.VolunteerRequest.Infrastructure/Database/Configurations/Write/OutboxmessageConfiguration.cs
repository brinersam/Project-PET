using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectPet.VolunteerRequests.Infrastructure.Outbox;

namespace ProjectPet.VolunteerRequests.Infrastructure.Database.Configurations.Write;
public class OutboxmessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Type)
            .HasColumnName("type")
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(x => x.Payload)
            .HasColumnName("payload")
            .HasColumnType("jsonb")
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(x => x.OccuredOnUtc)
            .HasColumnName("occured_on_utc")
            .HasConversion(
                push => push.ToUniversalTime(),
                pull => DateTime.SpecifyKind(pull, DateTimeKind.Utc)
            )
            .IsRequired();

        var processedOnUtcColumnName = "processed_on_utc";
        builder.Property(x => x.ProcessedOnUtc)
            .HasColumnName(processedOnUtcColumnName)
            .IsRequired(false);

        builder.Property(x => x.Error)
            .HasColumnName("error")
            .IsRequired(false);

        builder.HasIndex(i => new { i.OccuredOnUtc, i.ProcessedOnUtc })
            .HasDatabaseName("idx_outbox_unprocessed")
            .IncludeProperties(i => new { i.Id, i.Type, i.Payload })
            .HasFilter($"{processedOnUtcColumnName} IS NULL");
    }
}
