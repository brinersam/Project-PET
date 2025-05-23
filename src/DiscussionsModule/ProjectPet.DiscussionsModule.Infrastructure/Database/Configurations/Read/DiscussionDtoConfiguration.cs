using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectPet.DiscussionsModule.Contracts.Dto;
using ProjectPet.DiscussionsModule.Domain.Models;
using ProjectPet.Framework.EFExtensions;

namespace ProjectPet.DiscussionsModule.Infrastructure.Database.Configurations.Read;
public class DiscussionDtoConfiguration : IEntityTypeConfiguration<DiscussionDto>
{
    public void Configure(EntityTypeBuilder<DiscussionDto> builder)
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
