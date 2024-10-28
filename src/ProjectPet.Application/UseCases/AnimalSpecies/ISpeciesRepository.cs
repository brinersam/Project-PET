using CSharpFunctionalExtensions;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.UseCases.AnimalSpecies;

public interface ISpeciesRepository
{
    Task<Result<Guid, Error>> AddAsync(Species species, CancellationToken cancellationToken = default);
    Task<Result<Guid, Error>> Save(Species species, CancellationToken cancellationToken = default);
    Task<Result<Species, Error>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<Guid, Error>> Delete(Species species, CancellationToken cancellationToken = default);
}