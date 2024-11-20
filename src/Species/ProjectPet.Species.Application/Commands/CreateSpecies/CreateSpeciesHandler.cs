using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;
using ProjectPet.SpeciesModule.Application.Interfaces;

namespace ProjectPet.SpeciesModule.Application.Commands.CreateSpecies;

public class CreateSpeciesHandler
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<CreateSpeciesHandler> _logger;

    public CreateSpeciesHandler(
        ISpeciesRepository speciesRepository,
        ILogger<CreateSpeciesHandler> logger)
    {
        _speciesRepository = speciesRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> HandleAsync(
        CreateSpeciesCommand cmd,
        CancellationToken cancellationToken)
    {
        var doSpeciesExist = await _speciesRepository.ExistsByNameAsync(cmd.Name, cancellationToken);
        if (doSpeciesExist)
            return Error.Validation("value.not.unique",
                $"Can not add a duplicate species with name {cmd.Name}!");

        var newSpeciesRes = Species.Create(
            Guid.NewGuid(),
            cmd.Name);

        if (newSpeciesRes.IsFailure)
            return newSpeciesRes.Error;

        await _speciesRepository.AddAsync(newSpeciesRes.Value, cancellationToken);
        await _speciesRepository.Save(newSpeciesRes.Value, cancellationToken);

        _logger.LogInformation("Created new species entry with id {id}", newSpeciesRes.Value.Id);

        return newSpeciesRes.Value.Id;
    }
}