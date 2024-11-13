using Microsoft.EntityFrameworkCore;
using ProjectPet.Application.Dto;

namespace ProjectPet.Application.Database;

public interface IReadDbContext
{
    public DbSet<SpeciesReadDto> Species { get; }
    public DbSet<VolunteerReadDto> Volunteers { get; }
}
