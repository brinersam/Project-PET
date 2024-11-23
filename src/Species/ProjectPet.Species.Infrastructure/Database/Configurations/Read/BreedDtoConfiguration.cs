using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectPet.SpeciesModule.Contracts.Dto;

namespace ProjectPet.SpeciesModule.Infrastructure.Database.Configurations.Read;
public class BreedDtoConfiguration : IEntityTypeConfiguration<BreedDto>
{
    public void Configure(EntityTypeBuilder<BreedDto> builder)
    {
        builder.ToTable("breeds");

        builder.HasKey(x => x.Id);

        builder.HasOne<SpeciesDto>()
            .WithMany()
            .HasForeignKey(x => x.SpeciesId);
    }
}
