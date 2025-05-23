using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectPet.Core.Abstractions;
using ProjectPet.VolunteerRequests.Application.Interfaces;
using ProjectPet.VolunteerRequests.Infrastructure.Database;
using ProjectPet.VolunteerRequests.Infrastructure.Repositories;

namespace ProjectPet.VolunteerRequests.Infrastructure;
public static class DependencyInjection
{
    public static IHostApplicationBuilder AddVolunteerRequestModuleInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IReadDbContext, ReadDbContext>();

        builder.Services.AddScoped<WriteDbContext>();

        builder.Services.AddScoped<IVolunteerRequestRepository, VolunteerRequestRepository>();

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        return builder;
    }
}
