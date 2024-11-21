using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectPet.SpeciesModule.Application.Commands.CreateBreed;
using ProjectPet.SpeciesModule.Application.Commands.CreateSpecies;
using ProjectPet.SpeciesModule.Application.Commands.DeleteBreed;
using ProjectPet.SpeciesModule.Application.Commands.DeleteSpecies;
using ProjectPet.SpeciesModule.Application.Queries.GetAllBreedsById;
using ProjectPet.SpeciesModule.Application.Queries.GetAllSpecies;
using ProjectPet.SpeciesModule.Infrastructure;

namespace ProjectPet.SpeciesModule.Presentation;
public static class DependencyInjection
{
    public static IHostApplicationBuilder AddSpeciesModule(this IHostApplicationBuilder builder)
    {
        return builder.
            AddSpeciesInfrastructure();
    }

    private static IHostApplicationBuilder AddSpeciesHandlers(this IHostApplicationBuilder builder)
    {
        // write
        builder.Services.AddScoped<CreateSpeciesHandler>();
        builder.Services.AddScoped<DeleteSpeciesHandler>();
        builder.Services.AddScoped<GetAllSpeciesHandler>();

        builder.Services.AddScoped<CreateBreedsHandler>();
        builder.Services.AddScoped<DeleteBreedsHandler>();
        builder.Services.AddScoped<GetAllBreedsByIdHandler>();

        return builder;
    }
}