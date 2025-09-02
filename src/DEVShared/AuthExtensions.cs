using DEVShared.HttpFilters;
using DEVShared.Middlewares;
using DEVShared.SecretKeyAuthentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjectPet.Framework.Authorization;

namespace DEVShared;
public static class AuthExtensions
{
    public static IHostApplicationBuilder AddAuth(this IHostApplicationBuilder builder)
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
                     Id = "Bearer",
                   },
                 },
                 Array.Empty<string>()
               },
            });
        });

        return builder;
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
