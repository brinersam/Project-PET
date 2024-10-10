using Serilog.Events;
using Serilog;
using ProjectPet.Application.UseCases.Volunteers;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using FluentValidation;

namespace ProjectPet.API
{
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
        public static IServiceCollection AddAutoValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(typeof(IVolunteerRepository).Assembly);
            return services;
        }
    }
}
