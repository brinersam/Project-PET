namespace ProjectPet.Application.UseCases.AnimalSpecies.CreateBreed;

public record CreateBreedsRequest(
    Guid SpeciesId,
    string BreedName);