using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectPet.Core.Abstractions;
using ProjectPet.SharedKernel.Dto;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.SpeciesModule.Application.Queries.GetAllBreedsById;

public class GetAllBreedsByIdHandler
{
    private readonly IReadDbContext _readDbContext;
    public GetAllBreedsByIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }
    public async Task<Result<List<BreedDto>, Error>> HandleAsync(GetAllBreedsByIdQuery query, CancellationToken cancellationToken)
    {
        var exists = await _readDbContext.Species
            .Where(x => x.Id == query.SpeciesId)
            .AnyAsync(cancellationToken);


        if (exists == false)
            return Error.NotFound("value.not.found", $"Species with id {query.SpeciesId} was not found!");

        var result = await _readDbContext.Breeds
            .Where(x => x.SpeciesId == query.SpeciesId)
            .ToListAsync(cancellationToken);

        return result;
    }
}