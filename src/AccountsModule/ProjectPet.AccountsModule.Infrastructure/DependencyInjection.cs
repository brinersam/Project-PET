using DEVShared;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjectPet.AccountsModule.Application.Interfaces;
using ProjectPet.AccountsModule.Application.Services;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.AccountsModule.Infrastructure.Database;
using ProjectPet.AccountsModule.Infrastructure.EventConsumers.VolunteerRequestApproved;
using ProjectPet.AccountsModule.Infrastructure.HttpFilters;
using ProjectPet.AccountsModule.Infrastructure.Options;
using ProjectPet.AccountsModule.Infrastructure.Repositories;
using ProjectPet.AccountsModule.Infrastructure.SecretKeyAuthentication;
using ProjectPet.AccountsModule.Infrastructure.Seeding;
using ProjectPet.Core.Database;
using ProjectPet.Framework.Authorization;
using ProjectPet.Web.MIddlewares;

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

    public static IHostApplicationBuilder AddAuthInfrastructure(this IHostApplicationBuilder builder) // todo move out into shared framework
    {
        builder.Services.Configure<OptionsTokens>(
                builder.Configuration.GetRequiredSection(OptionsTokens.SECTION));
        var options = builder.Configuration.Get<OptionsTokens>();

        builder.Services
            .AddAuthorization(ConfigureAuthorizationOptions)
            .AddAuthentication(ConfigureAuthenticationOptions)
            .AddJwtBearer(x => ConfigureRsaTokenValidationOptions(x, builder))
            .AddScheme<SecretKeyAuthenticationOptions, SecretKeyAuthenticationHandler>("SecretKey", opts => opts.ExpectedKey = options.SecretKey);

        builder.Services.AddScoped<IAuthorizationHandler, PermissionRequirementHandler>();
        builder.Services.AddScoped<IAuthorizationHandler, SecretKeyAuthorizationHandler>();
        builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProviderWSecretKey>();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<UserScopedData>();
        builder.Services.AddScoped<ScopedUserDataMiddleware>();

        builder.Services.AddTransient<JwtForwardingHttpHandler>();
        builder.Services.AddSingleton<IHttpMessageHandlerBuilderFilter, JwtForwardingHttpFilter>();

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

    public static IHostApplicationBuilder AddAuthenticatedSwaggerGen(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Scheme = "bearer",
                Name = "Authorization",
                Description = "Please insert JWT token into field (no bearer prefix)",
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
               {
                 new OpenApiSecurityScheme
                 {
                   Reference = new OpenApiReference
                   {
                     Type = ReferenceType.SecurityScheme,
                     Id = "Bearer"
                   }
                 },
                  Array.Empty<string>()
                }
            });
        });

        return builder;
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

    private static void ConfigureAuthenticationOptions(AuthenticationOptions options)
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    }

    private static void ConfigureAuthorizationOptions(AuthorizationOptions options)
    {
        options.AddPolicy("IsAuthorized", policy =>
            policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser());
    }

    private static void ConfigureRsaTokenValidationOptions(JwtBearerOptions options, IHostApplicationBuilder builder)
    {
        var tokenOptions = builder.Configuration.Get<OptionsTokens>();

        var rsaKeyProvider = new RsaKeyProvider(false);
        var rsaKey = rsaKeyProvider.GetPublicRsa();
        var key = new RsaSecurityKey(rsaKey);

        options.TokenValidationParameters = TokenValidationParametersFactory.Create(tokenOptions, key, true);
    }
}

public class PermissionPolicyProviderWSecretKey : IAuthorizationPolicyProvider
{
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return Task.FromResult
        (
             new AuthorizationPolicyBuilder
                 (
                    JwtBearerDefaults.AuthenticationScheme,
                    "SecretKey"
                 )
                 .RequireAuthenticatedUser()
                 .Build()
        );
    }

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return Task.FromResult<AuthorizationPolicy>(null);
    }

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (string.IsNullOrWhiteSpace(policyName))
            return Task.FromResult<AuthorizationPolicy>(null);

        var policy = new AuthorizationPolicyBuilder
            (
                JwtBearerDefaults.AuthenticationScheme,
                "SecretKey"
            )
            .RequireAuthenticatedUser()
            .AddRequirements(new PermissionAttribute(policyName))
            .Build();

        return Task.FromResult<AuthorizationPolicy?>(policy);
    }
}