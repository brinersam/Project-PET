using Microsoft.EntityFrameworkCore;
using ProjectPet.VolunteerRequests.Contracts.Dto;

namespace ProjectPet.VolunteerRequests.Application.Interfaces;
public interface IReadDbContext
{
    DbSet<VolunteerRequestDto> VolunteerRequests { get; }
}
