using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectPet.Core.Extensions;
using ProjectPet.SpeciesModule.Domain.Models;

namespace ProjectPet.SpeciesModule.Infrastructure.Database.Configurations.Write;

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
            .HasForeignKey(x => x.SpeciesId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}
