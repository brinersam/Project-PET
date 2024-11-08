using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectPet.Application.UseCases.AnimalSpecies.CreateBreed;
using ProjectPet.Application.UseCases.AnimalSpecies.CreateSpecies;
using ProjectPet.Application.UseCases.AnimalSpecies.DeleteBreed;
using ProjectPet.Application.UseCases.AnimalSpecies.DeleteSpecies;
using ProjectPet.Application.UseCases.Volunteers.CreatePet;
using ProjectPet.Application.UseCases.Volunteers.CreateVolunteer;
using ProjectPet.Application.UseCases.Volunteers.DeleteVolunteer;
using ProjectPet.Application.UseCases.Volunteers.UpdateVolunteerInfo;
using ProjectPet.Application.UseCases.Volunteers.UpdateVolunteerPayment;
using ProjectPet.Application.UseCases.Volunteers.UpdateVolunteerSocials;
using ProjectPet.Application.UseCases.Volunteers.UploadPetPhoto;

namespace ProjectPet.Application;

public static class Inject
{
    public static IHostApplicationBuilder AddApplication(this IHostApplicationBuilder builder)
    {
        return builder
            .AddSpeciesHandlers()
            .AddVolunteerHandlers();
    }

    public static IHostApplicationBuilder AddSpeciesHandlers(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<CreateBreedsHandler>();
        builder.Services.AddScoped<CreateSpeciesHandler>();
        builder.Services.AddScoped<DeleteBreedsHandler>();
        builder.Services.AddScoped<DeleteSpeciesHandler>();
        return builder;
    }
    public static IHostApplicationBuilder AddVolunteerHandlers(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<CreateVolunteerHandler>();
        builder.Services.AddScoped<UpdateVolunteerInfoHandler>();
        builder.Services.AddScoped<UpdateVolunteerPaymentHandler>();
        builder.Services.AddScoped<UpdateVolunteerSocialsHandler>();
        builder.Services.AddScoped<DeleteVolunteerHandler>();

        builder.Services.AddScoped<CreatePetHandler>();
        builder.Services.AddScoped<UploadPetPhotoHandler>();
        return builder;
    }
}
