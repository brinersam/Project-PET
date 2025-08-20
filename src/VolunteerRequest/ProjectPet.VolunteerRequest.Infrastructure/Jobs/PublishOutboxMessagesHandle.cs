using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using ProjectPet.AccountsModule.Contracts;
using ProjectPet.VolunteerRequests.Infrastructure.Database;
using ProjectPet.VolunteerRequests.Infrastructure.Outbox;
using System.Text.Json;

namespace ProjectPet.VolunteerRequests.Infrastructure.Jobs;
public class PublishOutboxMessagesHandle
{
    private readonly IPublishEndpoint _publisher;
    private readonly ILogger<PublishOutboxMessagesJob> _logger;
    private readonly WriteDbContext _context;

    public PublishOutboxMessagesHandle(
        IPublishEndpoint publisher,
        ILogger<PublishOutboxMessagesJob> logger,
        WriteDbContext context)
    {
        _publisher = publisher;
        _logger = logger;
        _context = context;
    }

    public async Task HandleAsync(CancellationToken cancellationToken)
    {
        var messages = await _context
            .OutboxMessages
            .Where(x => x.ProcessedOnUtc == null)
            .Take(10)
            .OrderBy(x => x.OccuredOnUtc)
            .ToListAsync(cancellationToken);

        if (messages.Count <= 0)
            return;

        var maxRetries = 3;

        var pipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = maxRetries,
                BackoffType = DelayBackoffType.Exponential,
                Delay = TimeSpan.FromSeconds(2),
                ShouldHandle = new PredicateBuilder().Handle<Exception>(ex => ex is not NullReferenceException),
                OnRetry = retryArgs =>
                {
                    _logger.LogCritical(retryArgs.Outcome.Exception, "Attempt {p1} out of {p2}", retryArgs.AttemptNumber, maxRetries);
                    return ValueTask.CompletedTask;
                },

            })
            .Build();

        var publishTasks = messages.Select(x => PublishMessageAsync(x, pipeline, cancellationToken));
        await Task.WhenAll(publishTasks);

        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task PublishMessageAsync(OutboxMessage msg, ResiliencePipeline pipeline, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();

        try
        {
            await pipeline.ExecuteAsync(
                async token =>
                {
                    Type messageType = AssemblyReference.Assembly.GetType(msg.Type) ?? throw new NullReferenceException("Couldnt parse outboxmessage type");
                    var payload = JsonSerializer.Deserialize(msg.Payload, messageType) ?? throw new NullReferenceException("Couldnt parse outboxmessage payload");

                    await _publisher.Publish(payload, messageType, token);
                    _logger.LogTrace("Published message (Id: {p1})", msg.Id);
                },
                cancellation
            );
        }
        catch (Exception ex)
        {
            msg.Error = ex.Message;
            _logger.LogError(ex, "Failed to publish message (Id: {p1})", msg.Id);
        }
        finally
        {
            msg.ProcessedOnUtc = DateTime.UtcNow;
        }
    }
}
