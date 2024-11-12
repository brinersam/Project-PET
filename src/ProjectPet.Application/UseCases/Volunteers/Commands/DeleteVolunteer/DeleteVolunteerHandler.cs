using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.Application.Repositories;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.UseCases.Volunteers.Commands.DeleteVolunteer;

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

        var result = await _volunteerRepository.Delete(volunteerRes.Value, cancellationToken);

        _logger.LogInformation("Volunteer with id {id} was deleted successfully!", request.Id);

        return result.Value;
    }
}
