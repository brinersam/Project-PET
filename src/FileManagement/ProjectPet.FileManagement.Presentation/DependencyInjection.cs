using Microsoft.Extensions.Hosting;
using ProjectPet.FileManagement.Infrastructure;

namespace ProjectPet.FileManagement.Presentation;
public static class DependencyInjection
{
    public static IHostApplicationBuilder AddFileManagementModule(this IHostApplicationBuilder builder)
    {
        builder.AddFileManagementInfrastructure();
        return builder;
    }
}
