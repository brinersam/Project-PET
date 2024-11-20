using ProjectPet.Application.UseCases.AnimalSpecies.Commands.CreateBreed;
using ProjectPet.Core.Abstractions;

namespace ProjectPet.SpeciesModule.Presentation.Requests;

public record CreateBreedsRequest(string BreedName) : IToCommand<CreateBreedsCommand, Guid>
{
    public CreateBreedsCommand ToCommand(Guid speciesId)
    {
        return new CreateBreedsCommand(speciesId, BreedName);
    }
}
