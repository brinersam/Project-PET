using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProjectPet.Core.Abstractions;
using ProjectPet.VolunteerModule.Infrastructure.Database;
using Testcontainers.PostgreSql;
namespace ProjectPet.VolunteerModule.IntegrationTests.Factories;
public class IntegrationTestsWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private const string DB_CONNECTION_STRING_KEY = "CStrings:Postgresql";

    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("project_pet_tests")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        using var scope = Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigureServices);
        builder.ConfigureAppConfiguration(ConfigureConfigs);
    }

    private void ConfigureConfigs(WebHostBuilderContext context, IConfigurationBuilder builder)
    {
        builder.AddInMemoryCollection(new Dictionary<string, string?> 
        { 
            [DB_CONNECTION_STRING_KEY] = _dbContainer.GetConnectionString() 
        });
    }

    protected void ConfigureServices(IServiceCollection services)
    {
        services.RemoveAll<IDatabaseSeeder>();
    }

    new public async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }
}
