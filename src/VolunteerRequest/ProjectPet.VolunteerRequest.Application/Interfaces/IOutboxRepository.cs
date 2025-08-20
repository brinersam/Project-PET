
namespace ProjectPet.VolunteerRequests.Application.Interfaces;
public interface IOutboxRepository
{
    Task AddAsync<T>(T message, CancellationToken cancellationToken);
}
