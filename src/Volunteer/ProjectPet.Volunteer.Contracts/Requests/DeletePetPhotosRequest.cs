namespace ProjectPet.VolunteerModule.Contracts.Requests;

public record DeletePetPhotosRequest(string[] photoPathsToDelete);
