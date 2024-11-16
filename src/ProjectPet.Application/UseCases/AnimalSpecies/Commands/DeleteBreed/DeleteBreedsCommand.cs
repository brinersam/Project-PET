namespace ProjectPet.Application.UseCases.AnimalSpecies.Commands.DeleteBreed;

public record DeleteBreedsCommand(
    Guid SpeciesId,
    Guid BreedId);
