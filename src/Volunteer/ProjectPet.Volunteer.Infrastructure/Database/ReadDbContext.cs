using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjectPet.Core.Abstractions;
using ProjectPet.SharedKernel.Dto;

namespace ProjectPet.VolunteerModule.Infrastructure.Database;

public class ReadDbContext(IConfiguration configuration) : DbContext, IReadDbContext
{
    private readonly string DATABASE = configuration[Constants.DATABASE]
        ?? throw new ArgumentNullException(Constants.DATABASE);

    public DbSet<SpeciesDto> Species => Set<SpeciesDto>();
    public DbSet<VolunteerDto> Volunteers => Set<VolunteerDto>();
    public DbSet<BreedDto> Breeds => Set<BreedDto>();
    public DbSet<PetDto> Pets => Set<PetDto>();

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
