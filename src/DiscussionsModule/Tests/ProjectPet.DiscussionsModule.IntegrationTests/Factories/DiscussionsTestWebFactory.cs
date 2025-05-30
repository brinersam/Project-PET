using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProjectPet.Core.Database;
using ProjectPet.DiscussionsModule.Infrastructure.Database;

namespace ProjectPet.DiscussionsModule.IntegrationTests.Factories;
public class DiscussionsTestWebFactory : IntegrationTestWebFactoryBase
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.RemoveAll<IDatabaseSeeder>();
        base.ConfigureServices(services);
    }

    protected override IEnumerable<(Type DbContextType, string Schema)> IncludedDBContextsAndSchemas()
        => [
            (typeof(WriteDbContext), "discussion-module")
           ];
}
