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
            .HasForeignKey(x => x.volunteer_id);

        builder.HasOne<SpeciesDto>()
            .WithMany()
            .HasForeignKey(x => x.AnimalDataSpeciesID);


        builder.HasOne<BreedDto>()
            .WithMany()
            .HasForeignKey(x => x.AnimalDataBreedID);
    }
}
