using CSharpFunctionalExtensions;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SharedKernel.ValueObjects;
using ProjectPet.VolunteerRequests.Application.Interfaces;
using ProjectPet.VolunteerRequests.Domain.Models;

namespace ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.Update;
public class UpdateHandler
{
    private readonly IVolunteerRequestRepository _requestRepository;

    public UpdateHandler(
        IVolunteerRequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
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

        return requestRes.Value.Id;
    }
}
