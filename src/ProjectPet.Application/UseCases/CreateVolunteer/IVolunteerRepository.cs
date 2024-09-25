using CSharpFunctionalExtensions;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.UseCases.CreateVolunteer
{
    public interface IVolunteerRepository
    {
        Task<Result<Guid, Error>> AddAsync(Volunteer volunteer, CancellationToken cancellationToken = default);
        Task<Result<Volunteer, Error>> GetAsync(Guid id, CancellationToken cancellationToken = default);
    }
}