using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectPet.Application.Dto;

namespace ProjectPet.Infrastructure.Configurations.Read;
public class SpeciesDtoConfiguration : IEntityTypeConfiguration<SpeciesReadDto>
{
    public void Configure(EntityTypeBuilder<SpeciesReadDto> builder)
    {
        builder.ToTable("species");

        builder.HasKey(x => x.Id);

        builder.HasMany<BreedReadDto>(x => x.RelatedBreeds)
            .WithOne()
            .HasForeignKey("species-id")
            .OnDelete(DeleteBehavior.NoAction);
    }
}
