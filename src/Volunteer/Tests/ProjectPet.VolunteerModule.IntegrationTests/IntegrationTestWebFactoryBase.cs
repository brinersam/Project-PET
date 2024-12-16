using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using ProjectPet.Core.Abstractions;
using ProjectPet.VolunteerModule.Infrastructure.Database;
using Respawn;
using System.Data.Common;
using Testcontainers.PostgreSql;
namespace ProjectPet.VolunteerModule.IntegrationTests;
abstract public class IntegrationTestWebFactoryBase : WebApplicationFactory<Program>, IAsyncLifetime
{
    private const string DB_CONNECTION_STRING_KEY = "CStrings:Postgresql";

    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("project_pet_tests")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private Respawner _respawner = default!;
    private DbConnection _dbConnection = default!;

    public virtual async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());

        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        await InitializeRespawner();
    }

    private async Task InitializeRespawner()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = IncludedDBSchemas()
        });
    }

    abstract protected string[] IncludedDBSchemas();

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
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

    protected virtual void ConfigureServices(IServiceCollection services)
    {
        services.RemoveAll<IDatabaseSeeder>();
    }

    new public async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
        await base.DisposeAsync();
    }
}
