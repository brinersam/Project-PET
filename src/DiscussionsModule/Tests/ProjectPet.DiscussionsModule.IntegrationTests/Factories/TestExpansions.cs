using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectPet.DiscussionsModule.IntegrationTests.Factories;
public static class TestExpansions
{
    public static IEnumerable<RelationalDatabaseCreator> DbContextTypeToCreator(
        this IEnumerable<Type> types,
        IServiceScope scope)
    {
        foreach (var type in types)
            yield return (RelationalDatabaseCreator)
                            ((DbContext)
                                scope.ServiceProvider.GetRequiredService(type))
                                    .Database.GetService<IDatabaseCreator>();
    }
}
