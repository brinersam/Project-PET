using Microsoft.Extensions.DependencyInjection;
using Minio;
using ProjectPet.Application.UseCases.Volunteers;
using ProjectPet.Infrastructure.Repositories;

namespace ProjectPet.Infrastructure
{
    public static class Inject
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ApplicationDbContext>();
            services.AddScoped<IVolunteerRepository, VolunteerRepository>();

            services.AddMinio(options =>
            {
                options.WithEndpoint("epoint");
                options.WithCredentials("minioadmin", "minioadmin");
                options.WithSSL(false);
            });

            return services;
        }
    }
}
