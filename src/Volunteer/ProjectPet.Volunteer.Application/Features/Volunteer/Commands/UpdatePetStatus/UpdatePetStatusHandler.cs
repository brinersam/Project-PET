using CSharpFunctionalExtensions;
using ProjectPet.VolunteerModule.Domain.Models;
using ProjectPet.VolunteerModule.Application.Interfaces;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.UpdatePetStatus;

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
        if (cmd.Status == PetStatus.NotSet || cmd.Status == PetStatus.Home_Found)
            return Error.Validation("invalid.value", $"Pet status can not be set to {cmd.Status.ToString()}!");

        var volunteerRes = await _volunteerRepository.GetByIdAsync(cmd.VolunteerId, cancellationToken);
        if (volunteerRes.IsFailure)
            return volunteerRes.Error;
        var volunteer = volunteerRes.Value;

        volunteer.SetPetStatus(cmd.Petid, cmd.Status);

        await _volunteerRepository.Save(volunteer, cancellationToken);

        return Result.Success<Error>();
    }
}