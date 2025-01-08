using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerModule.Application.Interfaces;
using ProjectPet.VolunteerModule.Contracts.Dto;
using ProjectPet.VolunteerModule.Domain.Models;

namespace ProjectPet.VolunteerModule.Application.Features.Pets.Queries.GetPetById;

public class GetPetByIdHandler
{
    private readonly IReadDbContext _readDbContext;

    public GetPetByIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<PetDto, Error>> HandleAsync(
        GetPetByIdQuery query,
        CancellationToken cancellationToken)
    {
        var pet = await _readDbContext.Pets.FirstOrDefaultAsync(
            x => x.Id == query.Petid,
            cancellationToken);

        if (pet is null)
            return Errors.General.NotFound(typeof(Pet));

        return pet;
    }
}
