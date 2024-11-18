using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectPet.Application.Database;
using ProjectPet.Application.Dto;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.UseCases.Volunteers.Queries.GetPetById;
public record GetPetByIdQuery(Guid VolunteerId, Guid Petid);

public class GetPetByIdHandler
{
    private readonly IReadDbContext _readDbContext;

    public GetPetByIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }
    public async Task<Result<PetDto, Error>> HandleAsync(GetPetByIdQuery query, CancellationToken cancellationToken)
    {
        var volunteer = await _readDbContext.Volunteers.FirstOrDefaultAsync(x => x.Id == query.VolunteerId, cancellationToken);
        if (volunteer is null)
            return Errors.General.NotFound(typeof(Volunteer));

        var pet = await _readDbContext.Pets.FirstOrDefaultAsync(x => x.Id == query.Petid, cancellationToken);
        if (pet is null)
            return Errors.General.NotFound(typeof(Pet));

        return pet;
    }
}
