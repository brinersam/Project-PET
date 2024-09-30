using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjectPet.Domain.Models;

namespace ProjectPet.Infrastructure
{
    public class ApplicationDbContext(IConfiguration configuration) : DbContext
    {
        private readonly string DATABASE = configuration["CStrings:Postgresql"]
            ?? throw new ArgumentNullException("CStrings:Postgresql");

        public DbSet<Species> Species => Set<Species>();
        public DbSet<Volunteer> Volunteers => Set<Volunteer>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString(DATABASE));
            optionsBuilder.UseSnakeCaseNamingConvention();
            optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
            optionsBuilder.EnableSensitiveDataLogging();
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
}
