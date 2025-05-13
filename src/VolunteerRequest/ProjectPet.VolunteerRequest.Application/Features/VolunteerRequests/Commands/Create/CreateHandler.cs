using CSharpFunctionalExtensions;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SharedKernel.ValueObjects;
using ProjectPet.VolunteerRequests.Application.Interfaces;
using ProjectPet.VolunteerRequests.Domain.Models;

namespace ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.Create;
public class CreateHandler
{
    private readonly IVolunteerRequestRepository _requestRepository;

    public CreateHandler(
        IVolunteerRequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
    }

    public async Task<Result<Guid, Error>> HandleAsync(
        CreateCommand command,
        CancellationToken cancellationToken)
    {
        VolunteerAccountData volunteerAccount = new VolunteerAccountData(
            command.VolunteerAccountDto.PaymentInfos.Select(dto => new PaymentInfo(dto.Title, dto.Instructions)),
            command.VolunteerAccountDto.Experience,
            command.VolunteerAccountDto.Certifications);

        var createResult = VolunteerRequest.Create(
            command.UserId,
            Guid.Empty,
            volunteerAccount);
        if (createResult.IsFailure)
            return createResult.Error;


        var addResult = await _requestRepository.AddAsync(createResult.Value, cancellationToken);
        if (addResult.IsFailure)
            return addResult.Error;

        return createResult.Value.Id;
    }
}
