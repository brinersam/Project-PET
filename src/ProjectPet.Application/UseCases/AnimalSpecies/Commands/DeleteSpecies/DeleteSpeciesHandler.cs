using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.Application.Database;
using ProjectPet.Application.Repositories;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.UseCases.AnimalSpecies.Commands.DeleteSpecies;

public class DeleteSpeciesHandler
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<DeleteSpeciesHandler> _logger;

    public DeleteSpeciesHandler(
        ISpeciesRepository speciesRepository,
        IReadDbContext readDbContext,
        ILogger<DeleteSpeciesHandler> logger)
    {
        _speciesRepository = speciesRepository;
        _readDbContext = readDbContext;
        _logger = logger;
    }

    public async Task<UnitResult<Error>> HandleAsync(
        DeleteSpeciesCommand request,
        CancellationToken cancellationToken = default)
    {
        var getSpeciesRes = await _speciesRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (getSpeciesRes.IsFailure)
            return getSpeciesRes.Error;

        var isSpeciesInUse = _readDbContext.Pets.Any(x => x.AnimalDataSpeciesID == request.Id);

        if (isSpeciesInUse)
            return Error.Conflict("illegal.state", "Can not delete species that is currently in use by at least one pet!");

        var speciesAggregate = getSpeciesRes.Value;

        await _speciesRepository.DeleteAndSaveChangesAsync(speciesAggregate, cancellationToken);

        return Result.Success<Error>();
    }
}