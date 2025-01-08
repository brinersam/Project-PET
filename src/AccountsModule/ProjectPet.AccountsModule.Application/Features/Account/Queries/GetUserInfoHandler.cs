using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectPet.AccountsModule.Application.Interfaces;
using ProjectPet.AccountsModule.Contracts.Dto;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.AccountsModule.Application.Features.Account.Queries;

public class GetUserInfoHandler
{
    private readonly IReadDbContext _readDbContext;

    public GetUserInfoHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<UserDto, Error>> HandleAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var userInfo = await _readDbContext.Users.FirstOrDefaultAsync(
            x => x.Id == userId,
            cancellationToken);

        if (userInfo is null)
            return Errors.General.NotFound(typeof(User));

        return userInfo;
    }
}