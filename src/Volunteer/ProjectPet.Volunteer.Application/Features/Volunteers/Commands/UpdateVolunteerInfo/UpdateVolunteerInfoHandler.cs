using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.VolunteerModule.Domain.Models;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerModule.Application.Interfaces;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.UpdateVolunteerInfo;

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
        UpdateVolunteerInfoCommand request,
        CancellationToken cancellationToken = default)
    {
        var volunteerRes = await _volunteerRepository.GetByIdAsync(request.Id, cancellationToken);
        if (volunteerRes.IsFailure)
            return volunteerRes.Error;

        Phonenumber number = null!;
        if (request.Dto.Phonenumber != null)
        {
            number = Phonenumber.Create(
                request.Dto.Phonenumber.Phonenumber,
                request.Dto.Phonenumber.PhonenumberAreaCode).Value;
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
