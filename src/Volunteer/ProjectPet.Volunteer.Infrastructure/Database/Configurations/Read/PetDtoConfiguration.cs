using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectPet.SpeciesModule.Contracts.Dto;
using ProjectPet.VolunteerModule.Contracts.Dto;

namespace ProjectPet.VolunteerModule.Infrastructure.Database.Configurations.Read;
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

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted"); ;

        builder.HasQueryFilter(m => EF.Property<bool>(m, "_isDeleted") == false);
    }
}
