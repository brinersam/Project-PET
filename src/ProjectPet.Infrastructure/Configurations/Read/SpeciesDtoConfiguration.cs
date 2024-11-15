﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectPet.Application.Dto;

namespace ProjectPet.Infrastructure.Configurations.Read;
public class SpeciesDtoConfiguration : IEntityTypeConfiguration<SpeciesDto>
{
    public void Configure(EntityTypeBuilder<SpeciesDto> builder)
    {
        builder.ToTable("species");

        builder.HasKey(x => x.Id);

        builder.HasMany<BreedDto>(x => x.RelatedBreeds)
            .WithOne()
            .HasForeignKey("species-id")
            .OnDelete(DeleteBehavior.NoAction);
    }
}
