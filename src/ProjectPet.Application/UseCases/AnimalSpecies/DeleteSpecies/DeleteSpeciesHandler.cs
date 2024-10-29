using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.UseCases.AnimalSpecies.DeleteSpecies;

public class DeleteSpeciesHandler
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<DeleteSpeciesHandler> _logger;

    public DeleteSpeciesHandler(
        ISpeciesRepository speciesRepository,
        ILogger<DeleteSpeciesHandler> logger)
    {
        _speciesRepository = speciesRepository;
        _logger = logger;
    }

    public async Task<UnitResult<Error>> HandleAsync(
        DeleteSpeciesRequest request,
        CancellationToken cancellationToken = default)
    {
        var getSpeciesRes = await _speciesRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (getSpeciesRes.IsFailure)
            return getSpeciesRes.Error;

        var speciesAggregate = getSpeciesRes.Value;

        await _speciesRepository.DeleteAndSaveChangesasync(speciesAggregate, cancellationToken);

        return Result.Success<Error>();
    }
}