using CSharpFunctionalExtensions;
using ProjectPet.VolunteerModule.Domain.Models;
using ProjectPet.VolunteerModule.Application.Interfaces;
using ProjectPet.SharedKernel.ErrorClasses;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.UpdatePetStatus;

public class UpdatePetStatusHandler
{
    private readonly IVolunteerRepository _volunteerRepository;

    public UpdatePetStatusHandler(IVolunteerRepository volunteerRepository)
    {
        _volunteerRepository = volunteerRepository;
    }

    public async Task<UnitResult<Error>> HandleAsync(
        UpdatePetStatusCommand cmd,
        CancellationToken cancellationToken)
    {
        var status = (PetStatus)cmd.Status;
        if (Enum.IsDefined(typeof(PetStatus), status) == false)
            return Error.Validation("invalid.value", $"Invalid status: \"{status}\"");

        if (status == PetStatus.NotSet || status == PetStatus.Home_Found)
            return Error.Validation("invalid.value", $"Pet status can not be set to {cmd.Status}!");

        var volunteerRes = await _volunteerRepository.GetByIdAsync(cmd.VolunteerId, cancellationToken);
        if (volunteerRes.IsFailure)
            return volunteerRes.Error;
        var volunteer = volunteerRes.Value;

        volunteer.SetPetStatus(cmd.Petid, status);

        await _volunteerRepository.Save(volunteer, cancellationToken);

        return Result.Success<Error>();
    }
}