namespace ProjectPet.Application.UseCases.Volunteers.Commands.DeletePetPhotos;

public record DeletePetPhotosCommand(Guid volunteerId, Guid Petid, string[] PhotoPathsToDelete);
