using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjectPet.Core.Options;
using ProjectPet.VolunteerModule.Application.Interfaces;
using ProjectPet.VolunteerModule.Contracts.Dto;

namespace ProjectPet.VolunteerModule.Infrastructure.Database;

public class ReadDbContext(IConfiguration configuration) : DbContext, IReadDbContext
{
    private readonly string DATABASE = configuration[configuration.GetRequiredSection(OptionsDb.SECTION).Get<OptionsDb>()!.CStringKey];
    public DbSet<VolunteerDto> Volunteers => Set<VolunteerDto>();
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

        modelBuilder.HasDefaultSchema("volunteer");
    }

    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => builder.AddConsole());
    }

}
