using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectPet.VolunteerModule.Application.Features.Pets.Queries.GetPetById;
using ProjectPet.VolunteerModule.Application.Features.Pets.Queries.GetPetsPaginated;
using ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.CreatePet;
using ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.CreateVolunteer;
using ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.DeletePet;
using ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.DeletePetPhotos;
using ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.DeleteVolunteer;
using ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.PatchPet;
using ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.SetMainPetPhoto;
using ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.UpdatePetStatus;
using ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.UpdateVolunteerInfo;
using ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.UpdateVolunteerPayment;
using ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.UpdateVolunteerSocials;
using ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.UploadPetPhoto;
using ProjectPet.VolunteerModule.Application.Features.Volunteer.Queries.GetVolunteerById;
using ProjectPet.VolunteerModule.Application.Features.Volunteer.Queries.GetVolunteers;
using ProjectPet.VolunteerModule.Infrastructure;

namespace ProjectPet.VolunteerModule.Presentation;
public static class DependencyInjection
{
    public static IHostApplicationBuilder AddVolunteerModule(this IHostApplicationBuilder builder)
    {
        return builder
            .AddPetHandlers()
            .AddVolunteerHandlers()
            .AddVolunteerInfrastructure();
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
        builder.Services.AddScoped<DeletePetHandler>();
        builder.Services.AddScoped<SetMainPetPhotoHandler>();

        // read
        builder.Services.AddScoped<GetPetByIdHandler>();
        builder.Services.AddScoped<GetPetsPaginatedHandler>();

        return builder;
    }
}
