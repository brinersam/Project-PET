using Quartz;

namespace ProjectPet.VolunteerRequests.Infrastructure.Jobs;

[DisallowConcurrentExecution]
public class PublishOutboxMessagesJob : IJob
{
    public static readonly JobKey Key = new JobKey(nameof(PublishOutboxMessagesJob));
    private readonly PublishOutboxMessagesHandle _handle;

    public PublishOutboxMessagesJob(PublishOutboxMessagesHandle handle)
    {
        _handle = handle;
    }

    public static ITriggerConfigurator GetTrigger(ITriggerConfigurator trigger, string cron = "0 0/1 * 1/1 * ? *")
    {
        return trigger
            .ForJob(Key)
            .WithIdentity($"{nameof(PublishOutboxMessagesJob)}Trigger")
            .WithCronSchedule(cron);
    }

    public async Task Execute(IJobExecutionContext context)
    {
        if (context.RefireCount > 20) // something went very wrong, no use of trying anymore
            return;

        await _handle.HandleAsync(context.CancellationToken);
    }
}
