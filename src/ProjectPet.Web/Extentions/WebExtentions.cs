using ProjectPet.AccountsModule.Infrastructure.Seeding;

namespace ProjectPet.Web.Extentions;

public static class WebExtentions
{
    public async static Task SeedDatabaseAsync(
        this WebApplication app,
        CancellationToken cancellationToken = default)
    {
        await app.Services.GetRequiredService<DatabaseAccountsSeeder>().SeedAsync(true, cancellationToken);
    }
}
