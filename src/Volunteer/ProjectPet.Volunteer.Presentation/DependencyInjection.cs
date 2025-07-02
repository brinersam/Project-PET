using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectPet.VolunteerModule.Application;
using ProjectPet.VolunteerModule.Application.Features.Pets.Queries.GetPetById;
using ProjectPet.VolunteerModule.Application.Features.Pets.Queries.GetPetsPaginated;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.CreatePet;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.CreateVolunteer;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.DeletePet;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.DeletePetPhotos;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.DeleteVolunteer;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.PatchPet;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.SetMainPetPhoto;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.UpdatePetStatus;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.UpdateVolunteerInfo;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.UploadPetPhoto;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Queries.GetVolunteerById;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Queries.GetVolunteers;
using ProjectPet.VolunteerModule.Contracts;

namespace ProjectPet.VolunteerModule.Presentation;
public static class DependencyInjection
{
    public static IHostApplicationBuilder AddVolunteerModuleHandlers(this IHostApplicationBuilder builder)
    {
        return builder
            .AddPetHandlers()
            .AddVolunteerHandlers()
            .AddContracts()
            .AddValidators();
    }

    private static IHostApplicationBuilder AddValidators(this IHostApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));

        return builder;
    }

    private static IHostApplicationBuilder AddContracts(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IVolunteerContract, VolunteerContractImplementations>();

        return builder;
    }

    private static IHostApplicationBuilder AddVolunteerHandlers(this IHostApplicationBuilder builder)
    {
        // write
        builder.Services.AddScoped<CreateVolunteerHandler>();
        builder.Services.AddScoped<UpdateVolunteerInfoHandler>();
        builder.Services.AddScoped<DeleteVolunteerHandler>();

        // read
        builder.Services.AddScoped<GetVolunteerPaginatedHandler>();
        builder.Services.AddScoped<GetVolunteerByIdHandler>();


        return builder;
    }
    private static IHostApplicationBuilder AddPetHandlers(this IHostApplicationBuilder builder)
    {
        // write
        builder.Services.AddScoped<CreatePetHandler>();
        builder.Services.AddScoped<BeginPetPhotosUploadHandler>();
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
