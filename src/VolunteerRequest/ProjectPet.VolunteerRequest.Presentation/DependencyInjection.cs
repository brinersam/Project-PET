using Microsoft.Extensions.Hosting;

namespace ProjectPet.VolunteerRequests.Presentation;
public static class DependencyInjection
{
    public static IHostApplicationBuilder AddVolunteerRequestsModuleHandlers(this IHostApplicationBuilder builder)
    {
        return builder;
    }

}
