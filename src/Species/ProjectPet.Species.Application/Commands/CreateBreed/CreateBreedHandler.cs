using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SpeciesModule.Application.Interfaces;
using ProjectPet.SpeciesModule.Domain.Models;

namespace ProjectPet.SpeciesModule.Application.Commands.CreateBreed;

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
    CreateBreedsCommand cmd,
    CancellationToken cancellationToken)
    {
        var getSpeciesRes = await _speciesRepository.GetByIdAsync(
            cmd.SpeciesId,
            cancellationToken);

        if (getSpeciesRes.IsFailure)
            return getSpeciesRes.Error;

        var speciesAggregate = getSpeciesRes.Value;

        var breedRes = Breed.Create(cmd.BreedName);
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