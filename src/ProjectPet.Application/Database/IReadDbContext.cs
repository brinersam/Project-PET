using Microsoft.EntityFrameworkCore;
using ProjectPet.Application.Dto;

namespace ProjectPet.Application.Database;

public interface IReadDbContext
{
    public DbSet<SpeciesDto> Species { get; }
    public DbSet<VolunteerDto> Volunteers { get; }
    public DbSet<BreedDto> Breeds { get; }
    public DbSet<PetDto> Pets { get; }

}
