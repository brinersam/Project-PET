using CSharpFunctionalExtensions;
using ProjectPet.DiscussionsModule.Domain.Models;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.DiscussionsModule.Application.Interfaces;
public interface IDiscussionsRepository
{
    Task<Result<Guid, Error>> AddAsync(Discussion discussion, CancellationToken cancellationToken = default);
    Task<Result<Guid, Error>> Save(Discussion discussion, CancellationToken cancellationToken = default);
    Task<Result<Discussion, Error>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<Guid, Error>> Delete(Discussion discussion, CancellationToken cancellationToken = default);
}
