using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.UseCases.Volunteers.UpdateVolunteerInfo;

public class UpdateVolunteerInfoHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<UpdateVolunteerInfoHandler> _logger;

    public UpdateVolunteerInfoHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<UpdateVolunteerInfoHandler> logger)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> HandleAsync(
        UpdateVolunteerInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerRes = await _volunteerRepository.GetByIdAsync(request.Id, cancellationToken);
        if (volunteerRes.IsFailure)
            return volunteerRes.Error;

        PhoneNumber number = null!;
        if (request.Dto.PhoneNumber != null)
        {
            number = PhoneNumber.Create(
                request.Dto.PhoneNumber.Phonenumber,
                request.Dto.PhoneNumber.PhonenumberAreaCode).Value;
        }

        volunteerRes.Value.UpdateGeneralInfo(
            request.Dto.FullName,
            request.Dto.Email,
            request.Dto.Description,
            request.Dto.YOExperience,
            number);

        var saveRes = await _volunteerRepository.Save(volunteerRes.Value, cancellationToken);
        if (saveRes.IsFailure)
            return saveRes.Error;


        _logger.LogInformation("Updated volunteer with id {id} successfully!", saveRes.Value);
        return saveRes.Value;
    }
}
