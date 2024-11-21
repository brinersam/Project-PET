using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectPet.SpeciesModule.Application.Interfaces;
using ProjectPet.SpeciesModule.Infrastructure.Repositories;

namespace ProjectPet.SpeciesModule.Infrastructure;
public static class DependencyInjection
{
    public static IHostApplicationBuilder AddSpeciesInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<ISpeciesRepository, SpeciesRepository>();
        return builder;
    }
}
