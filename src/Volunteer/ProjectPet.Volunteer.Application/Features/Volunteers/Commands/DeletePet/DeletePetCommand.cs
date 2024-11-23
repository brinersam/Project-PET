namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.DeletePet;

public record DeletePetCommand(Guid VolunteerId, Guid PetId, bool SoftDelete);
