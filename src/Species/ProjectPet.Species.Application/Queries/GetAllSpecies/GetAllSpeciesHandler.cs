using CSharpFunctionalExtensions;
using ProjectPet.Core.Abstractions;
using ProjectPet.Core.HelperModels;
using ProjectPet.Core.Extensions;
using ProjectPet.SharedKernel.Dto;
using ProjectPet.SharedKernel.ErrorClasses;

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