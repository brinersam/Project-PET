namespace ProjectPet.SpeciesModule.Application.Commands.CreateBreed;

public record CreateBreedsCommand(
    Guid SpeciesId,
    string BreedName);