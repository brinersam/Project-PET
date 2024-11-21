using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SpeciesModule.Application.Interfaces;

namespace ProjectPet.SpeciesModule.Application.Commands.DeleteSpecies;

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

        var isSpeciesInUse = true; //_readDbContext.Pets.Any(x => x.SpeciesID == request.Id); todo interact with other service

        if (isSpeciesInUse)
            return Error.Conflict("illegal.state", "Can not delete species that is currently in use by at least one pet!");

        var speciesAggregate = getSpeciesRes.Value;

        await _speciesRepository.DeleteAndSaveChangesAsync(speciesAggregate, cancellationToken);

        return Result.Success<Error>();
    }
}