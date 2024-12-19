using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectPet.AccountsModule.Application.Features.Account.Commands.UpdateAccountPayment;
using ProjectPet.AccountsModule.Application.Features.Account.Commands.UpdateAccountSocials;
using ProjectPet.AccountsModule.Application.Features.Auth.Commands.Login;
using ProjectPet.AccountsModule.Application.Features.Auth.Commands.Register;

namespace ProjectPet.AccountsModule.Presentation;
public static class DependencyInjection
{
    public static IHostApplicationBuilder AddAuthModuleHandlers(this IHostApplicationBuilder builder)
    {
        return builder
            .AddValidators()
            .AddAuthHandlers();
    }

    private static IHostApplicationBuilder AddAuthHandlers(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<RegisterHandler>();
        builder.Services.AddScoped<LoginHandler>();
        builder.Services.AddScoped<UpdateAccountPaymentHandler>();
        builder.Services.AddScoped<UpdateAccountSocialsHandler>();
        return builder;
    }
    private static IHostApplicationBuilder AddValidators(this IHostApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
        return builder;
    }
}
