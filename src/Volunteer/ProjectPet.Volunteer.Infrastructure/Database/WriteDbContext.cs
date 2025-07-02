using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProjectPet.Core.Options;
using ProjectPet.VolunteerModule.Domain.Models;
using ProjectPet.VolunteerModule.Infrastructure.Interceptors;

namespace ProjectPet.VolunteerModule.Infrastructure.Database;

public class WriteDbContext : DbContext
{
    public WriteDbContext(DbContextOptions<WriteDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Volunteer> Volunteers => Set<Volunteer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(WriteDbContext).Assembly,
            x => x.FullName!.Contains("Configurations.Write"));

        modelBuilder.HasDefaultSchema("volunteer");
    }
}

public static class WriteDbContextExtension
{
    public static IServiceCollection AddScopedWriteDbContext(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<PetPhotoDeletionInterceptor>()
            .AddDbContext<WriteDbContext>(
                (sp, options) =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    var dbOptions = configuration.GetRequiredSection(OptionsDb.SECTION).Get<OptionsDb>()!;
                    var connectionString = configuration[dbOptions.CStringKey];

                    options.UseNpgsql(connectionString);
                    options.UseSnakeCaseNamingConvention();
                    options.UseLoggerFactory(CreateLoggerFactory());
                    options.EnableSensitiveDataLogging();

                    // interceptors that support di
                    options.AddInterceptors(sp.GetRequiredService<PetPhotoDeletionInterceptor>());
                });
    }

    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => builder.AddConsole());
    }
}
