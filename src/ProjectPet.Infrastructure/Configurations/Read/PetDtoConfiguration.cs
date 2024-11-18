using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectPet.Application.Dto;

namespace ProjectPet.Infrastructure.Configurations.Read;
public class PetDtoConfiguration : IEntityTypeConfiguration<PetDto>
{
    public void Configure(EntityTypeBuilder<PetDto> builder)
    {
        builder.ToTable("pets");

        builder.HasKey(e => e.Id);

        builder.HasOne<VolunteerDto>()
            .WithMany()
            .HasForeignKey(x => x.VolunteerId);

        builder.HasOne<SpeciesDto>()
            .WithMany()
            .HasForeignKey(x => x.SpeciesID);


        builder.HasOne<BreedDto>()
            .WithMany()
            .HasForeignKey(x => x.BreedID);
    }
}
