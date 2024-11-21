using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectPet.Core.Abstractions;
using ProjectPet.SpeciesModule.Application.Interfaces;
using ProjectPet.SpeciesModule.Infrastructure.Database;
using ProjectPet.SpeciesModule.Infrastructure.Repositories;

namespace ProjectPet.SpeciesModule.Infrastructure;
public static class DependencyInjection
{
    public static IHostApplicationBuilder AddSpeciesInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IReadDbContext, ReadDbContext>();

        builder.Services.AddScoped<WriteDbContext>();

        builder.Services.AddScoped<ISpeciesRepository, SpeciesRepository>();

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        return builder;
    }
}
