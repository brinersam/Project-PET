using FluentValidation;
using ProjectPet.API.Contracts.FileManagement;
using ProjectPet.API.Validation;
using ProjectPet.Application.UseCases.Volunteers;
using Serilog;
using Serilog.Events;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace ProjectPet.API;

public static class Inject
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
    public static IServiceCollection AddCustomAutoValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<IVolunteerRepository>();
        services.AddValidatorsFromAssemblyContaining<UploadFileDtoValidator>();

        services.AddFluentValidationAutoValidation(config =>
            config.OverrideDefaultResultFactoryWith<CustomResultFactory>()
        );

        return services;
    }
}
