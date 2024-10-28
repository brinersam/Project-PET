using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectPet.Domain.Models;

namespace ProjectPet.Infrastructure.Configurations;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.ToTable("species");

        builder.HasKey(x => x.Id);

        builder.ComplexProperty(x => x.SpeciesId, ba =>
        {
            ba.Property(s => s.Value)
                .IsRequired()
                .HasColumnName("species_id");
        });

        builder.Property(s => s.Name)
            .ConfigureString();

        builder.HasMany(x => x.RelatedBreeds)
            .WithOne()
            .HasForeignKey("breed_id")
            .OnDelete(DeleteBehavior.NoAction);
    }
}
