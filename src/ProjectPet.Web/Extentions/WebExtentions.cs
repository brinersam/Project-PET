﻿using ProjectPet.AccountsModule.Infrastructure.Seeding;
using ProjectPet.Core.Abstractions;

namespace ProjectPet.Web.Extentions;

public static class WebExtentions
{
    public async static Task SeedDatabaseAsync(
        this WebApplication app,
        CancellationToken cancellationToken = default)
    {
        var seeders = app.Services
            .CreateScope()
            .ServiceProvider
            .GetServices<IDatabaseSeeder>();

        foreach (var seeder in seeders)
            await seeder.SeedAsync(true, cancellationToken);
    }
}
