namespace ProjectPet.Application.UseCases.AnimalSpecies.Commands.CreateBreed;

public record CreateBreedsCommand(
    Guid SpeciesId,
    string BreedName);