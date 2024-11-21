using Microsoft.EntityFrameworkCore;
using ProjectPet.SharedKernel.Dto;

namespace ProjectPet.Core.Abstractions;

public interface IReadDbContext
{
    public DbSet<SpeciesDto> Species { get; }
    public DbSet<VolunteerDto> Volunteers { get; }
    public DbSet<BreedDto> Breeds { get; }
    public DbSet<PetDto> Pets { get; }

}
