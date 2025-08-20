using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProjectPet.AccountsModule.Infrastructure;
using ProjectPet.Core.Options;
using ProjectPet.VolunteerRequests.Infrastructure;
using ProjectPet.Web.ActionFilters;
using Quartz;
using Serilog;
using Serilog.Events;
using System.Collections.Specialized;

namespace ProjectPet.Web;

public static class RegisterServices
{
    public static IHostApplicationBuilder AddSerilogLogger(this IHostApplicationBuilder builder)
    {
        string seqConnectionString = builder.Configuration["CStrings:Seq"]
            ?? throw new ArgumentNullException("CStrings:Seq");

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.Seq(seqConnectionString)
            .Enrich.WithThreadId()
            .Enrich.WithEnvironmentName()
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
            .CreateLogger();

        builder.Services.AddSerilog();
        return builder;
    }

    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
        services.AddMvc(options =>
        {
            options.Filters.Add(typeof(FluentValidationFilter));
        });
        services.AddValidatorsFromAssemblyContaining<Program>();

        return services;
    }

    public static IHostApplicationBuilder ConfigureDbCstring(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<OptionsDb>(
            builder.Configuration.GetSection(OptionsDb.SECTION));

        return builder;
    }

    public static IHostApplicationBuilder AddRabbitMQ(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<RabbitMQOptions>(builder.Configuration.GetSection(RabbitMQOptions.REGION));
        builder.Services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            config.AddModuleConsumers();

            config.UsingRabbitMq((context, config) =>
            {
                var options = context.GetRequiredService<IOptions<RabbitMQOptions>>().Value;
                config.Host(
                    new Uri(options.Host),
                    configure =>
                    {
                        configure.Username(options.Username);
                        configure.Password(options.Password);
                    }
                );

                config.Durable = true;

                config.ConfigureEndpoints(context);
            });
        });
        return builder;

    }

    public static void AddModuleConsumers(this IBusRegistrationConfigurator config)
    {
        config.RegisterAccountsModuleConsumers();
    }

    public static IHostApplicationBuilder AddQuartzScheduler(this IHostApplicationBuilder builder)
    {
        builder.Services.AddQuartz(q =>
        {
            q.AddModuleQuartzJobs();
        });

        builder.Services.AddQuartzHostedService(q =>
        {
            q.WaitForJobsToComplete = true;
        });

        return builder;
    }

    public static void AddModuleQuartzJobs(this IServiceCollectionQuartzConfigurator config)
    {
        config.RegisterVolunteerRequestModuleJobs();
    }
}
