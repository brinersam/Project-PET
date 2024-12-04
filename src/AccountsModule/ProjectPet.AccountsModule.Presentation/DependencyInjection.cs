using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectPet.AccountsModule.Application.Features.Auth.Commands.Login;
using ProjectPet.AccountsModule.Application.Features.Auth.Commands.Register;

namespace ProjectPet.AccountsModule.Presentation;
public static class DependencyInjection
{
    public static IHostApplicationBuilder AddAuthModuleHandlers(this IHostApplicationBuilder builder)
    {
        return builder
            .AddAuthHandlers();
    }

    private static IHostApplicationBuilder AddAuthHandlers(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<RegisterHandler>();
        builder.Services.AddScoped<LoginHandler>();
        return builder;
    }
}
