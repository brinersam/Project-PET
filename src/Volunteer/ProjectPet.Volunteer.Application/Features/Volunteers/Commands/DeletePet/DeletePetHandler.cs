using CSharpFunctionalExtensions;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerModule.Application.Interfaces;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.DeletePet;

public class DeletePetHandler
{
    private readonly IVolunteerRepository _volunteerRepository;

    public DeletePetHandler(
        IVolunteerRepository volunteerRepository)
    {
        _volunteerRepository = volunteerRepository;
    }

    public async Task<UnitResult<Error>> HandleAsync(DeletePetCommand cmd, CancellationToken cancellationToken)
    {
        var volunteerRes = await _volunteerRepository.GetByIdAsync(cmd.VolunteerId, cancellationToken);
        if (volunteerRes.IsFailure)
            return volunteerRes.Error;
        var volunteer = volunteerRes.Value;

        if (cmd.SoftDelete)
        {
            var deletionResult = volunteer.SoftDeletePet(cmd.PetId);
            if (deletionResult.IsFailure)
                return deletionResult.Error;

            await _volunteerRepository.Save(volunteer, cancellationToken);

            return Result.Success<Error>();
        }
        else
        {
            var petRes = volunteer.GetPetById(cmd.PetId);
            if (petRes.IsFailure)
                return petRes.Error;

            var deletePetRes = volunteer.DeletePet(cmd.PetId);
            if (deletePetRes.IsFailure)
                return deletePetRes.Error;
        }

        return Result.Success<Error>();
    }
}