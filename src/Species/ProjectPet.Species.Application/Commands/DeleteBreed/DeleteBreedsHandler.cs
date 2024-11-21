using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.Core.Abstractions;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SpeciesModule.Application.Interfaces;

namespace ProjectPet.SpeciesModule.Application.Commands.DeleteBreed;

public class DeleteBreedsHandler
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<DeleteBreedsHandler> _logger;

    public DeleteBreedsHandler(
        ISpeciesRepository speciesRepository,
        IReadDbContext readDbContext,
        ILogger<DeleteBreedsHandler> logger)
    {
        _speciesRepository = speciesRepository;
        _readDbContext = readDbContext;
        _logger = logger;
    }

    public async Task<UnitResult<Error>> HandleAsync(
        DeleteBreedsCommand request,
        CancellationToken cancellationToken)
    {
        var getSpeciesRes = await _speciesRepository.GetByIdAsync(
            request.SpeciesId,
            cancellationToken);

        if (getSpeciesRes.IsFailure)
            return getSpeciesRes.Error;

        var speciesAggregate = getSpeciesRes.Value;

        var isBreedInUse = _readDbContext.Pets.Any(x => x.BreedID == request.BreedId);

        if (isBreedInUse)
            return Error.Conflict("illegal.state", "Can not delete breed that is currently in use by at least one pet!");

        speciesAggregate.RemoveBreed(request.BreedId);

        await _speciesRepository.Save(speciesAggregate, cancellationToken);

        return Result.Success<Error>();
    }
}