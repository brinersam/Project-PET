using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.UseCases.AnimalSpecies.DeleteBreed;

public class DeleteBreedsHandler
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<DeleteBreedsHandler> _logger;

    public DeleteBreedsHandler(
        ISpeciesRepository speciesRepository,
        ILogger<DeleteBreedsHandler> logger)
    {
        _speciesRepository = speciesRepository;
        _logger = logger;
    }

    public async Task<UnitResult<Error>> HandleAsync(
        DeleteBreedsRequest request,
        CancellationToken cancellationToken)
    {
        var getSpeciesRes = await _speciesRepository.GetByIdAsync(
            request.SpeciesId,
            cancellationToken);

        if (getSpeciesRes.IsFailure)
            return getSpeciesRes.Error;

        var speciesAggregate = getSpeciesRes.Value;

        speciesAggregate.RemoveBreed(request.BreedId);

        await _speciesRepository.Save(speciesAggregate, cancellationToken);

        return Result.Success<Error>();
    }
}