using Microsoft.EntityFrameworkCore;
using ProjectPet.VolunteerModule.Contracts.Dto;

namespace ProjectPet.VolunteerModule.Application.Interfaces;

public interface IReadDbContext
{
    public DbSet<VolunteerDto> Volunteers { get; }
    public DbSet<PetDto> Pets { get; }
}
