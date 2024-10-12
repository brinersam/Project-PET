using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Minio;
using Minio.AspNetCore;
using ProjectPet.Application.Database;
using ProjectPet.Application.UseCases.Volunteers;
using ProjectPet.Infrastructure.Options;
using ProjectPet.Infrastructure.Repositories;

namespace ProjectPet.Infrastructure
{
    public static class Inject
    {
        public static IHostApplicationBuilder AddInfrastructure(
            this IHostApplicationBuilder builder)
        {
            builder.AddMinio();

            builder.Services.AddScoped<ApplicationDbContext>();
            builder.Services.AddScoped<IVolunteerRepository, VolunteerRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            return builder;
        }

        private static IHostApplicationBuilder AddMinio(
            this IHostApplicationBuilder builder)
        {
            builder.Services.Configure<OptionsMinIO>(
                builder.Configuration.GetSection(OptionsMinIO.SECTION));

            builder.Services.AddMinio(options =>
            {
                var config = builder.Configuration.GetSection(OptionsMinIO.SECTION).Get<OptionsMinIO>() ?? 
                    throw new ArgumentNullException("Minio options not defined!");

                options.WithEndpoint(config.Endpoint);
                options.WithCredentials(config.AccessKey,config.SecretKey);
                options.WithSSL(config.WithSSL);
            });

            return builder;
        }
    }
}
