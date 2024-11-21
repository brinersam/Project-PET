using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectPet.SharedKernel.Dto;

namespace ProjectPet.VolunteerModule.Infrastructure.Database.Configurations.Read;
public class BreedDtoConfiguration : IEntityTypeConfiguration<BreedDto>
{
    public void Configure(EntityTypeBuilder<BreedDto> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne<SpeciesDto>()
            .WithMany()
            .HasForeignKey(x => x.SpeciesId);
    }
}
