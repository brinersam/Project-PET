using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProjectPet.AccountsModule.Application.Interfaces;
using ProjectPet.AccountsModule.Contracts.Dto;
using ProjectPet.Core.Options;

namespace ProjectPet.AccountsModule.Infrastructure.Database;

public class ReadDbContext(IConfiguration configuration, IOptions<OptionsDb> options) : DbContext, IReadDbContext
{
    private readonly string DATABASE = configuration[options.Value.CStringKey]!;

    public DbSet<UserDto> Users => Set<UserDto>();

    //public DbSet<Permission> Permissions => Set<Permission>();
    //public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    //public DbSet<RefreshSession> RefreshSessions => Set<RefreshSession>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(DATABASE);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ReadDbContext).Assembly,
            x => x.FullName!.Contains("Configurations.Read"));

        modelBuilder.HasDefaultSchema("auth");
    }

    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => builder.AddConsole());
    }
}
