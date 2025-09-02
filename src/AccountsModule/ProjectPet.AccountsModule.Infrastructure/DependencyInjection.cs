using DEVShared;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectPet.AccountsModule.Application.Interfaces;
using ProjectPet.AccountsModule.Application.Services;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.AccountsModule.Infrastructure.Database;
using ProjectPet.AccountsModule.Infrastructure.EventConsumers.VolunteerRequestApproved;
using ProjectPet.AccountsModule.Infrastructure.Options;
using ProjectPet.AccountsModule.Infrastructure.Repositories;
using ProjectPet.AccountsModule.Infrastructure.Seeding;
using ProjectPet.Core.Database;

namespace ProjectPet.AccountsModule.Infrastructure;
public static class DependencyInjection
{
    public static IHostApplicationBuilder AddAuthModuleInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.SetupAuthService();
        builder.AddEventConsumers();
        builder.Services.AddScoped<IDatabaseSeeder, DatabaseAccountsSeeder>();
        builder.Services.AddScoped<IAccountRepository, AccountRepository>();
        builder.Services.AddScoped<IPermissionModifierRepository, PermissionModifierRepository>();

        builder.Services.Configure<AdminCredsOptions>(builder.Configuration.GetSection(AdminCredsOptions.SECTION));

        return builder;
    }

    public static IHostApplicationBuilder AddEventConsumers(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<UpgradeAccountToVolunteer>();
        return builder;
    }

    public static IBusRegistrationConfigurator RegisterAccountsModuleConsumers(this IBusRegistrationConfigurator cfg)
    {
        cfg.AddConsumers(typeof(DependencyInjection).Assembly);
        return cfg;
    }

    private static IHostApplicationBuilder SetupAuthService(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<OptionsTokens>(
                builder.Configuration.GetRequiredSection(OptionsTokens.SECTION));
        var options = builder.Configuration.Get<OptionsTokens>();

        var rsaKeyProvider = new RsaKeyProvider(options.GenerateTokens);
        builder.Services.AddSingleton<IRsaKeyProvider>(rsaKeyProvider);

        builder.Services.AddTransient<TokenValidationParametersFactory>();

        builder.Services.AddScoped<AuthDbContext>();
        builder.Services.AddScoped<IReadDbContext, ReadDbContext>();
        builder.Services.AddScoped<IAuthRepository, AuthRepository>();

        builder.Services.AddTransient<ITokenProvider, TokenManager>();
        builder.Services.AddTransient<ITokenClaimsAccessor, TokenManager>();
        builder.Services.AddTransient<ITokenRefresher, TokenManager>();

        builder.Services
            .AddIdentity<User, Role>(ConfigureIdentityOptions)
            .AddEntityFrameworkStores<AuthDbContext>();

        return builder;
    }

    private static void ConfigureIdentityOptions(IdentityOptions options)
    {
        options.User.RequireUniqueEmail = true;
    }
}