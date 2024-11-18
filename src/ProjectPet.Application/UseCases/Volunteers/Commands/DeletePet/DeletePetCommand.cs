namespace ProjectPet.Application.UseCases.Volunteers.Commands.DeletePet;

public record DeletePetCommand(Guid VolunteerId, Guid PetId, bool SoftDelete);
