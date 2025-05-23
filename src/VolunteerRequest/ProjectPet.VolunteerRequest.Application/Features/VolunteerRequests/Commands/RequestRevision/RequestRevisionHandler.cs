using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerRequests.Application.Interfaces;

namespace ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.RequestRevision;
public class RequestRevisionHandler
{
    private readonly IVolunteerRequestRepository _requestRepository;
    private readonly ILogger<RequestRevisionHandler> _logger;

    public RequestRevisionHandler(
        IVolunteerRequestRepository requestRepository,
        ILogger<RequestRevisionHandler> logger)
    {
        _requestRepository = requestRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> HandleAsync(
        RequestRevisionCommand command,
        CancellationToken cancellationToken)
    {
        var requestRes = await _requestRepository.GetByIdAsync(command.RequestId, cancellationToken);
        if (requestRes.IsFailure)
            return requestRes.Error;

        var requestRevisionRes = requestRes.Value.RequestRevision(command.RevisionComment);
        if (requestRevisionRes.IsFailure)
            return requestRevisionRes.Error;

        var saveRes = await _requestRepository.Save(requestRes.Value, cancellationToken);
        if (saveRes.IsFailure)
            return saveRes.Error;

        _logger.LogInformation(
            "User (id {O2}) requested revision for volunteer request (id {O2})",
            requestRes.Value.UserId,
            requestRes.Value.Id);

        return requestRes.Value.Id;
    }
}