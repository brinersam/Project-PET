using Microsoft.Extensions.DependencyInjection;
using ProjectPet.Application.UseCases.Volunteers;

namespace ProjectPet.Application
{
    public static class Inject
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<CreateVolunteerHandler>();
            services.AddScoped<UpdateVolunteerInfoHandler>();
            services.AddScoped<UpdateVolunteerPaymentHandler>();
            services.AddScoped<UpdateVolunteerSocialsHandler>();
            services.AddScoped<DeleteVolunteerHandler>();

            return services;
        }
    }
}
