using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectPet.Core.Database;
using ProjectPet.VolunteerRequests.Application.Interfaces;
using ProjectPet.VolunteerRequests.Infrastructure.Database;
using ProjectPet.VolunteerRequests.Infrastructure.Jobs;
using ProjectPet.VolunteerRequests.Infrastructure.Outbox;
using ProjectPet.VolunteerRequests.Infrastructure.Repositories;
using Quartz;

namespace ProjectPet.VolunteerRequests.Infrastructure;
public static class DependencyInjection
{
    public static IHostApplicationBuilder AddVolunteerRequestModuleInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IReadDbContext, ReadDbContext>();

        builder.Services.AddScoped<WriteDbContext>();

        builder.Services.AddScoped<IOutboxRepository, OutboxRepository>();

        builder.Services.AddScoped<IVolunteerRequestRepository, VolunteerRequestRepository>();

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        builder.Services.AddScoped<PublishOutboxMessagesHandle>();

        return builder;
    }

    public static IServiceCollectionQuartzConfigurator RegisterVolunteerRequestModuleJobs(this IServiceCollectionQuartzConfigurator config)
    {
        config.AddJob<PublishOutboxMessagesJob>(options => options.WithIdentity(PublishOutboxMessagesJob.Key));
        config.AddTrigger(options => PublishOutboxMessagesJob.GetTrigger(options));

        return config;
    }
}
