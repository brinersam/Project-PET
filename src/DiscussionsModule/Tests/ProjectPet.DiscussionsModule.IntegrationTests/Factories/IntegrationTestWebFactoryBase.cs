using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using System.Data.Common;
using Testcontainers.PostgreSql;

namespace ProjectPet.DiscussionsModule.IntegrationTests.Factories;
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

        await InitializeDbContexts(scope);
        await InitializeRespawner();
    }

    private async Task InitializeDbContexts(IServiceScope scope)
    {
        var dbCreators = IncludedDBContextsAndSchemas()
            .Select(x => x.DbContextType)
            .DbContextTypeToCreator(scope);

        bool firstDbContext = true;
        foreach (var dbCreator in dbCreators)
        {
            if (firstDbContext)
            {
                firstDbContext = false;
                await dbCreator.EnsureDeletedAsync();
                await dbCreator.EnsureCreatedAsync();
                continue;
            }

            await dbCreator.CreateTablesAsync();
        }
    }

    private async Task InitializeRespawner()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = IncludedDBContextsAndSchemas().Select(x => x.Schema).ToArray(),
        });
    }

    abstract protected IEnumerable<(Type DbContextType, string Schema)> IncludedDBContextsAndSchemas();

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

    protected virtual void ConfigureServices(IServiceCollection services) { }

    new public async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
        await base.DisposeAsync();
    }
}
