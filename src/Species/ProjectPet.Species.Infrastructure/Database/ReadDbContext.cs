using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjectPet.Core.Options;
using ProjectPet.SpeciesModule.Application.Interfaces;
using ProjectPet.SpeciesModule.Contracts.Dto;

namespace ProjectPet.SpeciesModule.Infrastructure.Database;

public class ReadDbContext(IConfiguration configuration) : DbContext, IReadDbContext
{
    private readonly string DATABASE = configuration[configuration.GetSection(OptionsDb.SECTION).Get<OptionsDb>()!.CString];
    public DbSet<SpeciesDto> Species => Set<SpeciesDto>();
    public DbSet<BreedDto> Breeds => Set<BreedDto>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(DATABASE);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ReadDbContext).Assembly,
            x => x.FullName!.Contains("Configurations.Read"));
    }

    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => builder.AddConsole());
    }
}
