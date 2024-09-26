using ProjectPet.Domain.Models;

namespace ProjectPet.Application.UseCases.CreateVolunteer
{
    public interface IVolunteerRepository
    {
        Task<Guid> AddAsync(Volunteer volunteer, CancellationToken cancellationToken = default);
        Task<Volunteer> GetAsync(Guid id, CancellationToken cancellationToken = default);
    }
}