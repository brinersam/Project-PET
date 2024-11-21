using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectPet.SpeciesModule.Domain.Models;
using ProjectPet.Core.Extensions;

namespace ProjectPet.SpeciesModule.Infrastructure.Database.Configurations.Write;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.ToTable("breeds");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Value)
            .ConfigureString();
    }
}
