using CSharpFunctionalExtensions;
using DEVShared;
using MassTransit;
using Microsoft.Extensions.Logging;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SharedKernel.SharedDto;
using ProjectPet.VolunteerRequests.Application.Interfaces;

namespace ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.Approve;
public class ApproveHandler
{
    private readonly IVolunteerRequestRepository _requestRepository;
    private readonly IPublishEndpoint _publisher;
    private readonly ILogger<ApproveHandler> _logger;

    public ApproveHandler(
        IVolunteerRequestRepository requestRepository,
        IPublishEndpoint publisher,
        ILogger<ApproveHandler> logger)
    {
        _requestRepository = requestRepository;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task<UnitResult<Error>> HandleAsync(
        ApproveCommand command,
        CancellationToken cancellationToken)
    {
        var requestRes = await _requestRepository.GetByIdAsync(command.RequestId, cancellationToken);
        if (requestRes.IsFailure)
            return requestRes.Error;

        var approveRes = requestRes.Value.ApproveRequest();
        if (approveRes.IsFailure)
            return approveRes.Error;

        var data = new VolunteerAccountDto()
        {
            Certifications = requestRes.Value.VolunteerData.Certifications,
            Experience = requestRes.Value.VolunteerData.Experience,
            PaymentInfos = requestRes.Value.VolunteerData.PaymentInfos.Select(x => new PaymentInfoDto(x.Title, x.Instructions)).ToList(),
        };

        _publisher.Publish(new VolunteerRequestApprovedEvent(requestRes.Value.UserId, data), CancellationToken.None);

        var saveRes = await _requestRepository.Save(requestRes.Value, cancellationToken);
        if (saveRes.IsFailure)
            return saveRes.Error;

        _logger.LogInformation(
            "Volunteer request (id {O1}) was approved. Was assigned admin (id {O2})",
            requestRes.Value.Id,
            requestRes.Value.AdminId);

        return Result.Success<Error>();
    }
}