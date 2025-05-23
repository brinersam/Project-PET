using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectPet.DiscussionsModule.Application;
using ProjectPet.DiscussionsModule.Application.Features.Discussions.Commands.AddMessageToDiscussion;
using ProjectPet.DiscussionsModule.Application.Features.Discussions.Commands.CloseDiscussion;
using ProjectPet.DiscussionsModule.Application.Features.Discussions.Commands.DeleteMessage;
using ProjectPet.DiscussionsModule.Application.Features.Discussions.Commands.EditMessage;
using ProjectPet.DiscussionsModule.Application.Features.Discussions.Queries.GetDiscussion;
using ProjectPet.DiscussionsModule.Contracts;

namespace ProjectPet.DiscussionsModule.Presentation;
public static class DependencyInjection
{
    public static IHostApplicationBuilder AddDiscussionModuleHandlers(this IHostApplicationBuilder builder)
    {
        return builder
            .AddHandlers()
            .AddContractImplementation();
    }

    private static IHostApplicationBuilder AddContractImplementation(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IDiscussionModuleContract, DiscussionModuleContractImplementation>();
        return builder;
    }
    
    private static IHostApplicationBuilder AddHandlers(this IHostApplicationBuilder builder)
    {
        // commands
        builder.Services.AddScoped<AddMessageToDiscussionHandler>();
        builder.Services.AddScoped<CloseDiscussionHandler>();
        builder.Services.AddScoped<DeleteMessageHandler>();
        builder.Services.AddScoped<EditMessageHandler>();

        return builder;
    }
}
