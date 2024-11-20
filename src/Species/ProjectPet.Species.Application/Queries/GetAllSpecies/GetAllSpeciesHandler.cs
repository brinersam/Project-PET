using CSharpFunctionalExtensions;
using ProjectPet.Application.Database;
using ProjectPet.Application.Dto;
using ProjectPet.Application.Extensions;
using ProjectPet.Application.Models;
using ProjectPet.Domain.Shared;

namespace ProjectPet.SpeciesModule.Application.Queries.GetAllSpecies;

public class GetAllSpeciesHandler
{
    private readonly IReadDbContext _readDbContext;

    public GetAllSpeciesHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }
    public async Task<Result<PagedList<SpeciesDto>, Error>> HandleAsync(GetAllSpeciesQuery query, CancellationToken cancellationToken)
    {
        return await _readDbContext.Species.ToPagedListAsync(query, cancellationToken);
    }
}