using ProjectPet.Core.Abstractions;
using ProjectPet.SpeciesModule.Application.Commands.CreateBreed;

namespace ProjectPet.SpeciesModule.Presentation.Requests;

public record CreateBreedsRequest(string BreedName) : IToCommand<CreateBreedsCommand, Guid>
{
    public CreateBreedsCommand ToCommand(Guid speciesId)
    {
        return new CreateBreedsCommand(speciesId, BreedName);
    }
}
