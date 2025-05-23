using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectPet.DiscussionsModule.Contracts.Dto;
using ProjectPet.DiscussionsModule.Domain.Models;

namespace ProjectPet.DiscussionsModule.Infrastructure.Database.Configurations.Read;
public class MessageDtoConfiguration : IEntityTypeConfiguration<MessageDto>
{
    public void Configure(EntityTypeBuilder<MessageDto> builder)
    {
        builder.ToTable($"{nameof(Message)}s");

        builder.HasKey(x => x.Id);
    }
}
