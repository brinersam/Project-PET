using Microsoft.EntityFrameworkCore;
using ProjectPet.SpeciesModule.Contracts.Dto;

namespace ProjectPet.SpeciesModule.Application.Interfaces;

public interface IReadDbContext
{
    public DbSet<SpeciesDto> Species { get; }
    public DbSet<BreedDto> Breeds { get; }

}
