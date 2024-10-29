﻿using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.UseCases.AnimalSpecies.CreateBreed;

public class CreateBreedsHandler
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<CreateBreedsHandler> _logger;

    public CreateBreedsHandler(
        ISpeciesRepository speciesRepository,
        ILogger<CreateBreedsHandler> logger)
    {
        _speciesRepository = speciesRepository;
        _logger = logger;
    }


    public async Task<Result<Guid, Error>> HandleAsync(
    CreateBreedsRequest dto,
    CancellationToken cancellationToken)
    {
        var getSpeciesRes = await _speciesRepository.GetByIdAsync(
            dto.SpeciesId,
            cancellationToken);

        if (getSpeciesRes.IsFailure)
            return getSpeciesRes.Error;

        var speciesAggregate = getSpeciesRes.Value;

        var breedRes = Breed.Create(dto.BreedName);
        if (breedRes.IsFailure)
            return breedRes.Error;

        var newBreed = breedRes.Value;

        var addBreedRes = speciesAggregate.AddNewBreed(newBreed);
        if (addBreedRes.IsFailure)
            return addBreedRes.Error;

        await _speciesRepository.Save(speciesAggregate, cancellationToken);

        _logger.LogInformation("Added new breed {breed} to species {species}",
            newBreed.Value,
            speciesAggregate.Name);

        return newBreed.Id;
    }
}