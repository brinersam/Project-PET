using CSharpFunctionalExtensions;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.Repositories;

public interface ISpeciesRepository
{
    Task<Result<Guid, Error>> AddAsync(Species species, CancellationToken cancellationToken = default);
    Task<Result<Guid, Error>> Save(Species species, CancellationToken cancellationToken = default);
    Task<Result<Species, Error>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<Guid, Error>> DeleteAndSaveChangesasync(Species species, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
}