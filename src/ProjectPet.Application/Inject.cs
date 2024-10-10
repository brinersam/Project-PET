using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectPet.Application.UseCases.Volunteers;

namespace ProjectPet.Application
{
    public static class Inject
    {
        public static IHostApplicationBuilder AddApplication(this IHostApplicationBuilder builder)
        {
            builder.Services.AddScoped<CreateVolunteerHandler>();
            builder.Services.AddScoped<UpdateVolunteerInfoHandler>();
            builder.Services.AddScoped<UpdateVolunteerPaymentHandler>();
            builder.Services.AddScoped<UpdateVolunteerSocialsHandler>();
            builder.Services.AddScoped<DeleteVolunteerHandler>();

            return builder;
        }
    }
}
