using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectPet.Core.Database;
using ProjectPet.DiscussionsModule.Application.Interfaces;
using ProjectPet.DiscussionsModule.Infrastructure.Database;
using ProjectPet.DiscussionsModule.Infrastructure.Repositories;

namespace ProjectPet.DiscussionsModule.Infrastructure;
public static class DependencyInjection
{
    public static IHostApplicationBuilder AddDiscussionsModuleInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IReadDbContext, ReadDbContext>();

        builder.Services.AddScoped<WriteDbContext>();

        builder.Services.AddScoped<IDiscussionsRepository, DiscussionsRepository>();

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        return builder;
    }
}
