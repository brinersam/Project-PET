using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SpeciesModule.Application.Interfaces;
using ProjectPet.VolunteerModule.Contracts;

namespace ProjectPet.SpeciesModule.Application.Commands.DeleteSpecies;

public class DeleteSpeciesHandler
{
    private readonly IVolunteerContract _volunteerContract;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<DeleteSpeciesHandler> _logger;

    public DeleteSpeciesHandler(
        IVolunteerContract volunteerContract,
        ISpeciesRepository speciesRepository,
        IReadDbContext readDbContext,
        ILogger<DeleteSpeciesHandler> logger)
    {
        _volunteerContract = volunteerContract;
        _speciesRepository = speciesRepository;
        _readDbContext = readDbContext;
        _logger = logger;
    }

    public async Task<UnitResult<Error>> HandleAsync(
        DeleteSpeciesCommand request,
        CancellationToken cancellationToken = default)
    {
        var getSpeciesRes = await _speciesRepository.GetByIdAsync(
            request.SpeciesId,
            cancellationToken);

        if (getSpeciesRes.IsFailure)
            return getSpeciesRes.Error;


        var petRes = await _volunteerContract.GetPetBySpeciesIdAsync(request.SpeciesId, cancellationToken);

        if (petRes.IsSuccess)
            return Error.Conflict("illegal.state", "Can not delete species that is currently in use by at least one pet!");

        var speciesAggregate = getSpeciesRes.Value;

        await _speciesRepository.DeleteAndSaveChangesAsync(speciesAggregate, cancellationToken);

        return Result.Success<Error>();
    }
}