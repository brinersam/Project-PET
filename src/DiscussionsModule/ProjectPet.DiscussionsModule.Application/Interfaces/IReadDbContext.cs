using Microsoft.EntityFrameworkCore;
using ProjectPet.DiscussionsModule.Contracts.Dto;

namespace ProjectPet.DiscussionsModule.Application.Interfaces;
public interface IReadDbContext
{
    DbSet<DiscussionDto> Discussions { get; }
}
