using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectPet.Application.UseCases.AnimalSpecies.Commands.CreateBreed;
using ProjectPet.Application.UseCases.AnimalSpecies.Commands.CreateSpecies;
using ProjectPet.Application.UseCases.AnimalSpecies.Commands.DeleteBreed;
using ProjectPet.Application.UseCases.AnimalSpecies.Commands.DeleteSpecies;
using ProjectPet.Application.UseCases.AnimalSpecies.Queries.GetAllBreedsById;
using ProjectPet.Application.UseCases.AnimalSpecies.Queries.GetAllSpecies;
using ProjectPet.Application.UseCases.Volunteers.Commands.CreatePet;
using ProjectPet.Application.UseCases.Volunteers.Commands.CreateVolunteer;
using ProjectPet.Application.UseCases.Volunteers.Commands.DeletePetPhotos;
using ProjectPet.Application.UseCases.Volunteers.Commands.DeleteVolunteer;
using ProjectPet.Application.UseCases.Volunteers.Commands.PatchPet;
using ProjectPet.Application.UseCases.Volunteers.Commands.UpdatePetStatus;
using ProjectPet.Application.UseCases.Volunteers.Commands.UpdateVolunteerInfo;
using ProjectPet.Application.UseCases.Volunteers.Commands.UpdateVolunteerPayment;
using ProjectPet.Application.UseCases.Volunteers.Commands.UpdateVolunteerSocials;
using ProjectPet.Application.UseCases.Volunteers.Commands.UploadPetPhoto;
using ProjectPet.Application.UseCases.Volunteers.Queries.GetVolunteerById;
using ProjectPet.Application.UseCases.Volunteers.Queries.GetVolunteers;

namespace ProjectPet.Application;

public static class Inject
{
    public static IHostApplicationBuilder AddApplication(this IHostApplicationBuilder builder)
    {
        return builder
            .AddSpeciesHandlers()
            .AddVolunteerHandlers()
            .AddPetHandlers();
    }

    public static IHostApplicationBuilder AddSpeciesHandlers(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<CreateSpeciesHandler>();
        builder.Services.AddScoped<DeleteSpeciesHandler>();
        builder.Services.AddScoped<GetAllSpeciesHandler>();

        builder.Services.AddScoped<CreateBreedsHandler>();
        builder.Services.AddScoped<DeleteBreedsHandler>();
        builder.Services.AddScoped<GetAllBreedsByIdHandler>();

        return builder;
    }
    public static IHostApplicationBuilder AddVolunteerHandlers(this IHostApplicationBuilder builder)
    {
        // write
        builder.Services.AddScoped<CreateVolunteerHandler>();
        builder.Services.AddScoped<UpdateVolunteerInfoHandler>();
        builder.Services.AddScoped<UpdateVolunteerPaymentHandler>();
        builder.Services.AddScoped<UpdateVolunteerSocialsHandler>();
        builder.Services.AddScoped<DeleteVolunteerHandler>();

        // read
        builder.Services.AddScoped<GetVolunteerPaginatedHandler>();
        builder.Services.AddScoped<GetVolunteerByIdHandler>();


        return builder;
    }
    public static IHostApplicationBuilder AddPetHandlers(this IHostApplicationBuilder builder)
    {
        // write
        builder.Services.AddScoped<CreatePetHandler>();
        builder.Services.AddScoped<UploadPetPhotoHandler>();
        builder.Services.AddScoped<UpdatePetStatusHandler>();
        builder.Services.AddScoped<DeletePetPhotosHandler>();
        builder.Services.AddScoped<PatchPetHandler>();

        return builder;
    }
}
