using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectPet.DiscussionsModule.Application;
using ProjectPet.DiscussionsModule.Contracts;

namespace ProjectPet.DiscussionsModule.Presentation;
public static class DependencyInjection
{
    public static IHostApplicationBuilder AddDiscussionModuleHandlers(this IHostApplicationBuilder builder)
    {
        return builder
            .AddContractImplementation();
    }

    private static IHostApplicationBuilder AddContractImplementation(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IDiscussionModuleContract, DiscussionModuleContractImplementation>();
        return builder;
    }
}
