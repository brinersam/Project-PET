using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectPet.Application.Dto;

namespace ProjectPet.Infrastructure.Configurations.Read;
public class BreedDtoConfiguration : IEntityTypeConfiguration<BreedReadDto>
{
    public void Configure(EntityTypeBuilder<BreedReadDto> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
