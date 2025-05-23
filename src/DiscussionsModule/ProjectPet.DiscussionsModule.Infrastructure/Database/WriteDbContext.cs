using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjectPet.Core.Options;
using ProjectPet.DiscussionsModule.Domain.Models;
using ProjectPet.DiscussionsModule.Infrastructure.Database.Configurations.Write;

namespace ProjectPet.DiscussionsModule.Infrastructure.Database;

public class WriteDbContext(IConfiguration configuration) : DbContext
{
    private readonly string DATABASE = configuration[configuration.GetRequiredSection(OptionsDb.SECTION).Get<OptionsDb>()!.CStringKey];
    public DbSet<Discussion> Discussions => Set<Discussion>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(DATABASE);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DiscussionConfiguration());
        modelBuilder.ApplyConfiguration(new MessageConfiguration());

        modelBuilder.HasDefaultSchema("discussion-module");
    }

    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => builder.AddConsole());
    }
}
