using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerModule.Application.Interfaces;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.DeleteVolunteer;

public class DeleteVolunteerHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<DeleteVolunteerHandler> _logger;

    public DeleteVolunteerHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<DeleteVolunteerHandler> logger)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
    }
    public async Task<Result<Guid, Error>> HandleAsync(
        DeleteVolunteerCommand request,
        CancellationToken cancellationToken = default)
    {
        var volunteerRes = await _volunteerRepository.GetByIdAsync(request.Id, cancellationToken);
        if (volunteerRes.IsFailure)
            return volunteerRes.Error;

        Result<Guid, Error> result;

        if (request.SoftDelete)
            result = await _volunteerRepository.SoftDelete(volunteerRes.Value, cancellationToken);
        else
            result = await _volunteerRepository.Delete(volunteerRes.Value, cancellationToken);

        _logger.LogInformation("Volunteer with id {id} was {soft}deleted successfully!", request.Id, request.SoftDelete? "soft " : "");

        return result.Value;
    }
}
