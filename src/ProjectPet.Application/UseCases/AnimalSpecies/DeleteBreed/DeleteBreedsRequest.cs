namespace ProjectPet.Application.UseCases.AnimalSpecies.DeleteBreed;

public record DeleteBreedsRequest(
    Guid SpeciesId,
    Guid BreedId);
