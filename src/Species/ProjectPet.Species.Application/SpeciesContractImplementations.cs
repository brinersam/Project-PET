using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SpeciesModule.Application.Interfaces;
using ProjectPet.SpeciesModule.Contracts;
using ProjectPet.SpeciesModule.Contracts.Dto;

namespace ProjectPet.SpeciesModule.Application;

public class SpeciesContractImplementations : ISpeciesContract
{
    private readonly IReadDbContext _readDbContext;

    public SpeciesContractImplementations(
        IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<BreedDto, Error>> GetBreedByIdAsync(
        Guid breedId,
        CancellationToken cancellationToken = default)
    {
        var breedDtoRes = await _readDbContext.Breeds
            .FirstOrDefaultAsync(x => x.Id == breedId, cancellationToken);

        if (breedDtoRes is null)
            return Errors.General.NotFound(typeof(BreedDto));

        return breedDtoRes;
    }

    public async Task<Result<BreedDto, Error>> GetBreedByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var breedDtoRes = await _readDbContext.Breeds
            .FirstOrDefaultAsync(x => x.Value.Contains(name), cancellationToken);

        if (breedDtoRes is null)
            return Errors.General.NotFound(typeof(BreedDto));

        return breedDtoRes;
    }

    public async Task<Result<SpeciesDto, Error>> GetSpeciesByIdAsync(
        Guid speciesId,
        CancellationToken cancellationToken = default)
    {
        var speciesDtoRes = await _readDbContext.Species.
            FirstOrDefaultAsync(x => x.Id == speciesId, cancellationToken);

        if (speciesDtoRes is null)
            return Errors.General.NotFound(typeof(SpeciesDto));

        return speciesDtoRes;
    }

    public async Task<Result<SpeciesDto, Error>> GetSpeciesByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var speciesDtoRes = await _readDbContext.Species.
            FirstOrDefaultAsync(x => x.Name.Contains(name), cancellationToken);

        if (speciesDtoRes is null)
            return Errors.General.NotFound(typeof(SpeciesDto));

        return speciesDtoRes;
    }
}

