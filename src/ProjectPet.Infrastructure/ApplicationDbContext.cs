using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjectPet.Domain.Models;
using ProjectPet.Infrastructure.Interceptors;

namespace ProjectPet.Infrastructure;

public class ApplicationDbContext(IConfiguration configuration) : DbContext
{
    private readonly string DATABASE = configuration["CStrings:Postgresql"]
        ?? throw new ArgumentNullException("CStrings:Postgresql");

    public DbSet<Species> Species => Set<Species>();
    public DbSet<Volunteer> Volunteers => Set<Volunteer>();
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(DATABASE);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionsBuilder.EnableSensitiveDataLogging();

        optionsBuilder.AddInterceptors(new SoftDeleteInterceptor());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    private ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => builder.AddConsole());
    }
}
