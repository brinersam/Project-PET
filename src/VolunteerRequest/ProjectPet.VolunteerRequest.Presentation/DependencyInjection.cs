using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.Approve;
using ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.Create;
using ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.Reject;
using ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.RequestRevision;
using ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.Review;
using ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.Update;
using ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Queries.GetByAdminIdPaginatedFiltered;
using ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Queries.GetByUserIdPaginatedFiltered;
using ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Queries.GetUnassignedPaginated;

namespace ProjectPet.VolunteerRequests.Presentation;
public static class DependencyInjection
{
    public static IHostApplicationBuilder AddVolunteerRequestsModuleHandlers(this IHostApplicationBuilder builder)
    {
        return builder
            .AddVolunteerRequestsHandlers()
            .AddValidators();
    }

    private static IHostApplicationBuilder AddValidators(this IHostApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));

        return builder;
    }

    private static IHostApplicationBuilder AddVolunteerRequestsHandlers(this IHostApplicationBuilder builder)
    {
        // write
        builder.Services.AddScoped<ApproveHandler>();
        builder.Services.AddScoped<CreateHandler>();
        builder.Services.AddScoped<RejectHandler>();
        builder.Services.AddScoped<RequestRevisionHandler>();
        builder.Services.AddScoped<ReviewHandler>();
        builder.Services.AddScoped<UpdateHandler>();

        // read
        builder.Services.AddScoped<GetByAdminIdPaginatedFilteredHandler>();
        builder.Services.AddScoped<GetByUserIdPaginatedFilteredHandler>();
        builder.Services.AddScoped<GetUnassignedPaginatedHandler>();

        return builder;
    }
}
