using CSharpFunctionalExtensions;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SpeciesModule.Domain.Models;

namespace ProjectPet.SpeciesModule.Application.Interfaces;

public interface ISpeciesRepository
{
    Task<Result<Guid, Error>> AddAsync(Species species, CancellationToken cancellationToken = default);
    Task<Result<Guid, Error>> Save(Species species, CancellationToken cancellationToken = default);
    Task<Result<Species, Error>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<Guid, Error>> DeleteAndSaveChangesAsync(Species species, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
}