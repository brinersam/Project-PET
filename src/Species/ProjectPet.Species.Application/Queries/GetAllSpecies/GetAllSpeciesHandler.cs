using CSharpFunctionalExtensions;
using ProjectPet.Core.Extensions;
using ProjectPet.Core.HelperModels;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SpeciesModule.Application.Interfaces;
using ProjectPet.SpeciesModule.Contracts.Dto;

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