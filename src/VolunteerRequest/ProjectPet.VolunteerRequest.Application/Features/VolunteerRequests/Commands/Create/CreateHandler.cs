using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SharedKernel.ValueObjects;
using ProjectPet.VolunteerRequests.Application.Interfaces;
using ProjectPet.VolunteerRequests.Domain.Models;

namespace ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.Create;
public class CreateHandler
{
    private readonly IVolunteerRequestRepository _requestRepository;
    private readonly ILogger<CreateHandler> _logger;

    public CreateHandler(
        IVolunteerRequestRepository requestRepository,
        ILogger<CreateHandler> logger)
    {
        _requestRepository = requestRepository;
        _logger = logger;
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

        _logger.LogInformation(
            "Volunteer request (id {O1}) was created by user (id {O2})",
            createResult.Value.Id,
            createResult.Value.UserId);

        return createResult.Value.Id;
    }
}
