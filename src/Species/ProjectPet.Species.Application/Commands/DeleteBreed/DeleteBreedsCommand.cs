namespace ProjectPet.SpeciesModule.Application.Commands.DeleteBreed;

public record DeleteBreedsCommand(
    Guid SpeciesId,
    Guid BreedId);
