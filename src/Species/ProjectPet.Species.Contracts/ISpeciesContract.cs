using CSharpFunctionalExtensions;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SpeciesModule.Contracts.Dto;

namespace ProjectPet.SpeciesModule.Contracts;
public interface ISpeciesContract
{
    Task<Result<BreedDto, Error>> GetBreedByIdAsync(Guid breedId, CancellationToken cancellationToken = default);
    Task<Result<SpeciesDto, Error>> GetSpeciesByIdAsync(Guid speciesId, CancellationToken cancellationToken = default);
    Task<Result<BreedDto, Error>> GetBreedByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<Result<SpeciesDto, Error>> GetSpeciesByNameAsync(string name, CancellationToken cancellationToken = default);
}
