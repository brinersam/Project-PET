using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectPet.DiscussionsModule.Domain.Models;

namespace ProjectPet.DiscussionsModule.Infrastructure.Database.Configurations.Write;
public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable($"{nameof(Message)}s");

        builder.HasKey(x => x.Id);
    }
}
