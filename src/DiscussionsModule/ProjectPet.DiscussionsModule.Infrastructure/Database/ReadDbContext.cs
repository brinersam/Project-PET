using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjectPet.Core.Options;
using ProjectPet.DiscussionsModule.Application.Interfaces;
using ProjectPet.DiscussionsModule.Contracts.Dto;
using ProjectPet.DiscussionsModule.Infrastructure.Database.Configurations.Read;

namespace ProjectPet.DiscussionsModule.Infrastructure.Database;
public class ReadDbContext(IConfiguration configuration) : DbContext, IReadDbContext
{
    private readonly string DATABASE = configuration[configuration.GetRequiredSection(OptionsDb.SECTION).Get<OptionsDb>()!.CStringKey];
    public DbSet<DiscussionDto> Discussions => Set<DiscussionDto>();

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
        modelBuilder.ApplyConfiguration(new DiscussionDtoConfiguration());
        modelBuilder.ApplyConfiguration(new MessageDtoConfiguration());

        modelBuilder.HasDefaultSchema("discussion-module");
    }

    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => builder.AddConsole());
    }
}

