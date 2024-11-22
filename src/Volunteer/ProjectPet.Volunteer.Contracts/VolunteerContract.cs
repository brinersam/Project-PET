using CSharpFunctionalExtensions;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerModule.Contracts.Dto;

namespace ProjectPet.VolunteerModule.Contracts;
public interface IVolunteerContract
{
    Task<Result<PetDto, Error>> GetPetByBreedIdAsync(Guid breedId, CancellationToken cancellationToken = default);
    Task<Result<PetDto, Error>> GetPetBySpeciesIdAsync(Guid speciesId, CancellationToken cancellationToken = default);
}
