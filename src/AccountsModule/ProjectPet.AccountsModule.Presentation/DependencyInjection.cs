using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectPet.AccountsModule.Application;
using ProjectPet.AccountsModule.Application.Features.Account.Commands.UpdateAccountPayment;
using ProjectPet.AccountsModule.Application.Features.Account.Commands.UpdateAccountSocials;
using ProjectPet.AccountsModule.Application.Features.Account.Queries;
using ProjectPet.AccountsModule.Application.Features.Auth.Commands.Login;
using ProjectPet.AccountsModule.Application.Features.Auth.Commands.RefreshTokens;
using ProjectPet.AccountsModule.Application.Features.Auth.Commands.Register;
using ProjectPet.AccountsModule.Contracts;

namespace ProjectPet.AccountsModule.Presentation;
public static class DependencyInjection
{
    public static IHostApplicationBuilder AddAuthModuleHandlers(this IHostApplicationBuilder builder)
    {
        return builder
            .AddValidators()
            .AddAuthHandlers()
            .AddContractImplementation();
    }

    private static IHostApplicationBuilder AddAuthHandlers(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<RegisterHandler>();
        builder.Services.AddScoped<LoginHandler>();
        builder.Services.AddScoped<UpdateAccountPaymentHandler>();
        builder.Services.AddScoped<UpdateAccountSocialsHandler>();
        builder.Services.AddScoped<GetUserInfoHandler>();
        builder.Services.AddScoped<RefreshTokensHandler>();

        return builder;
    }
    private static IHostApplicationBuilder AddValidators(this IHostApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
        return builder;
    }

    private static IHostApplicationBuilder AddContractImplementation(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAccountsModuleContract, AccountsModuleContractImplementation>();
        return builder;
    }
}
