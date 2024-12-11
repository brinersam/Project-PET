using ProjectPet.Core.Abstractions;
using ProjectPet.SpeciesModule.Domain.Requests;

namespace ProjectPet.SpeciesModule.Application.Commands.CreateBreed;

public record CreateBreedsCommand(Guid SpeciesId, string BreedName)
    : IMapFromRequest<CreateBreedsCommand, CreateBreedsRequest, Guid>
{
    public static CreateBreedsCommand FromRequest(CreateBreedsRequest request, Guid speciesId)
            => new CreateBreedsCommand(speciesId, request.BreedName);
}


