using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SharedKernel.ValueObjects;
using ProjectPet.VolunteerRequests.Application.Interfaces;
using ProjectPet.VolunteerRequests.Domain.Models;

namespace ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.Update;
public class UpdateHandler
{
    private readonly IVolunteerRequestRepository _requestRepository;
    private readonly ILogger<UpdateHandler> _logger;

    public UpdateHandler(
        IVolunteerRequestRepository requestRepository,
        ILogger<UpdateHandler> logger)
    {
        _requestRepository = requestRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> HandleAsync(
        UpdateCommand command,
        CancellationToken cancellationToken)
    {
        var requestRes = await _requestRepository.GetByIdAsync(command.RequestId, cancellationToken);
        if (requestRes.IsFailure)
            return requestRes.Error;

        VolunteerAccountData volunteerAccount = new VolunteerAccountData(
            command.VolunteerAccountDto.PaymentInfos.Select(dto => new PaymentInfo(dto.Title, dto.Instructions)),
            command.VolunteerAccountDto.Experience,
            command.VolunteerAccountDto.Certifications);

        var setDataRes = requestRes.Value.UpdateVolunteerData(volunteerAccount);
        if (setDataRes.IsFailure)
            return setDataRes.Error;

        var saveRes = await _requestRepository.Save(requestRes.Value, cancellationToken);
        if (saveRes.IsFailure)
            return saveRes.Error;

        _logger.LogInformation(
            "Volunteer request (id {O1}) was updated by user (id {O2})",
            requestRes.Value.Id,
            requestRes.Value.UserId);

        return requestRes.Value.Id;
    }
}
