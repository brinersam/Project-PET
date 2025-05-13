using CSharpFunctionalExtensions;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerRequests.Domain.Models;

namespace ProjectPet.VolunteerRequests.Application.Interfaces;
public interface IVolunteerRequestRepository
{
    Task<Result<Guid, Error>> AddAsync(VolunteerRequest volunteer, CancellationToken cancellationToken = default);
    Task<Result<Guid, Error>> Save(VolunteerRequest volunteer, CancellationToken cancellationToken = default);
    Task<Result<VolunteerRequest, Error>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<Guid, Error>> Delete(VolunteerRequest volunteer, CancellationToken cancellationToken = default);
}
