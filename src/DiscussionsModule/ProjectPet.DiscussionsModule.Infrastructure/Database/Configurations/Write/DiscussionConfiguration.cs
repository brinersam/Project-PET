using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectPet.Core.Extensions;
using ProjectPet.DiscussionsModule.Domain.Models;

namespace ProjectPet.DiscussionsModule.Infrastructure.Database.Configurations.Write;
public class DiscussionConfiguration : IEntityTypeConfiguration<Discussion>
{
    public void Configure(EntityTypeBuilder<Discussion> builder)
    {
        builder.ToTable($"{nameof(Discussion)}s");
        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Messages)
            .WithOne()
            .HasForeignKey("discussion_id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.UserIds)
            .JsonVOCollectionConverter()
            .HasColumnName("userIds")
            .HasColumnType("jsonb")
            .IsRequired(false);
    }
}
