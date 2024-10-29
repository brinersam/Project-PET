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

        builder.Property(s => s.Name)
            .ConfigureString();

        builder.HasMany(x => x.RelatedBreeds)
            .WithOne()
            .HasForeignKey("SpeciesId")
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}
