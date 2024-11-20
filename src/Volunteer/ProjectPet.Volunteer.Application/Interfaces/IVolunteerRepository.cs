using CSharpFunctionalExtensions;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;

namespace ProjectPet.VolunteerModule.Application.Interfaces;

public interface IVolunteerRepository
{
    Task<Result<Guid, Error>> AddAsync(Volunteer volunteer, CancellationToken cancellationToken = default);
    Task<Result<Guid, Error>> Save(Volunteer volunteer, CancellationToken cancellationToken = default);
    Task<Result<Volunteer, Error>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<Guid, Error>> Delete(Volunteer volunteer, CancellationToken cancellationToken = default);
    Task<Result<Guid, Error>> SoftDelete(Volunteer volunteer, CancellationToken cancellationToken = default);
}