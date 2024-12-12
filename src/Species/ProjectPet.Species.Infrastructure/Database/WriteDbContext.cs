using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjectPet.Core.Options;
using ProjectPet.SpeciesModule.Domain.Models;

namespace ProjectPet.SpeciesModule.Infrastructure.Database;

public class WriteDbContext(IConfiguration configuration) : DbContext
{
    private readonly string DATABASE = configuration[configuration.GetRequiredSection(OptionsDb.SECTION).Get<OptionsDb>()!.CStringKey];
    public DbSet<Species> Species => Set<Species>();
    public DbSet<Breed> Breeds => Set<Breed>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(DATABASE);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(WriteDbContext).Assembly,
            x => x.FullName!.Contains("Configurations.Write"));

        modelBuilder.HasDefaultSchema("species");
    }

    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => builder.AddConsole());
    }
}
