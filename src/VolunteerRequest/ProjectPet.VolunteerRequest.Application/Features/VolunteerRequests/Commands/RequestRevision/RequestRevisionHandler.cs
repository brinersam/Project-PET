using CSharpFunctionalExtensions;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerRequests.Application.Interfaces;

namespace ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.RequestRevision;
public class RequestRevisionHandler
{
    private readonly IVolunteerRequestRepository _requestRepository;

    public RequestRevisionHandler(
        IVolunteerRequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
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

        return requestRes.Value.Id;
    }
}