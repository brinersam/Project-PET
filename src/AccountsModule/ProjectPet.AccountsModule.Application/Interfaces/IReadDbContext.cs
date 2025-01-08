using Microsoft.EntityFrameworkCore;
using ProjectPet.AccountsModule.Contracts.Dto;

namespace ProjectPet.AccountsModule.Application.Interfaces;
public interface IReadDbContext
{
    DbSet<UserDto> Users { get; }
}
