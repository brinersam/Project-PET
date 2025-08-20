using ProjectPet.VolunteerRequests.Application.Interfaces;
using ProjectPet.VolunteerRequests.Infrastructure.Database;
using System.Text.Json;

namespace ProjectPet.VolunteerRequests.Infrastructure.Outbox;
public class OutboxRepository : IOutboxRepository
{
    private readonly WriteDbContext _dbContext;

    public OutboxRepository(WriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync<T>(T message, CancellationToken cancellationToken) 
    {
        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            OccuredOnUtc = DateTime.Now,
            Type = typeof(T).FullName,
            Payload = JsonSerializer.Serialize(message),
        };

        await _dbContext.AddAsync(outboxMessage, cancellationToken);
    }
}
