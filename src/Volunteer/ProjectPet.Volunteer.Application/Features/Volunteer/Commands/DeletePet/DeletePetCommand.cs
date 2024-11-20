namespace ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.DeletePet;

public record DeletePetCommand(Guid VolunteerId, Guid PetId, bool SoftDelete);
