using ProjectPet.API.Etc;
using ProjectPet.Application.UseCases.AnimalSpecies.Commands.CreateBreed;

namespace ProjectPet.API.Requests.AnimalSpecies;

public record CreateBreedsRequest(string BreedName) : IToCommand<CreateBreedsCommand, Guid>
{
    public CreateBreedsCommand ToCommand(Guid speciesId)
    {
        return new CreateBreedsCommand(speciesId, BreedName);
    }
}
