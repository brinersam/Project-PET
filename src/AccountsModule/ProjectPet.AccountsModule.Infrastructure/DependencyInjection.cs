using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjectPet.AccountsModule.Application.Interfaces;
using ProjectPet.AccountsModule.Application.Services;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.AccountsModule.Infrastructure.Database;
using ProjectPet.AccountsModule.Infrastructure.Options;
using ProjectPet.AccountsModule.Infrastructure.Repositories;
using ProjectPet.AccountsModule.Infrastructure.Seeding;
using ProjectPet.Core.Abstractions;
using ProjectPet.Core.Options;
using ProjectPet.Framework.Authorization;
using System.Text;

namespace ProjectPet.AccountsModule.Infrastructure;
public static class DependencyInjection
{
    public static IHostApplicationBuilder AddAuthModuleInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.AddAuth();
        builder.Services.AddScoped<IDatabaseSeeder, DatabaseAccountsSeeder>();
        builder.Services.AddScoped<IAccountRepository, AccountRepository>();

        builder.Services.Configure<AdminCredsOptions>(builder.Configuration.GetSection(AdminCredsOptions.SECTION));

        return builder;
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

    private static IHostApplicationBuilder AddAuth(this IHostApplicationBuilder builder)
    {
        builder.Services.AddTransient<TokenValidationParametersFactory>();
        builder.Services.AddScoped<AuthDbContext>();
        builder.Services.AddScoped<IReadDbContext, ReadDbContext>();
        builder.Services.AddScoped<IAuthRepository, AuthRepository>();

        builder.Services.AddTransient<ITokenProvider, TokenManager>();
        builder.Services.AddTransient<ITokenClaimsAccessor, TokenManager>();
        builder.Services.AddTransient<ITokenRefresher, TokenManager>();

        builder.Services.Configure<OptionsTokens>(
                builder.Configuration.GetRequiredSection(OptionsTokens.SECTION));

        builder.Services
            .AddIdentity<User, Role>(ConfigureIdentityOptions)
            .AddEntityFrameworkStores<AuthDbContext>();

        builder.Services
            .AddAuthorization(ConfigureAuthorizationOptions)
            .AddAuthentication(ConfigureAuthenticationOptions)
            .AddJwtBearer(x => ConfigureTokenValidationOptions(x, builder));

        builder.Services.AddScoped<IAuthorizationHandler, PermissionRequirementHandler>();

        builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

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

    private static void ConfigureTokenValidationOptions(JwtBearerOptions options, IHostApplicationBuilder builder)
    {
        var optionsJwt = builder.Configuration.GetRequiredSection(OptionsTokens.SECTION).Get<OptionsTokens>();
        if (optionsJwt == null) 
            throw new ArgumentNullException(nameof(optionsJwt));
        options.TokenValidationParameters = TokenValidationParametersFactory.Create(optionsJwt);
    }
}
