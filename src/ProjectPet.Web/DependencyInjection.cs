using FluentValidation;
using Serilog;
using Serilog.Events;

namespace ProjectPet.Web;

public static class DependencyInjection
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
        services.AddValidatorsFromAssemblyContaining<Program>();
        return services;
    }
}
