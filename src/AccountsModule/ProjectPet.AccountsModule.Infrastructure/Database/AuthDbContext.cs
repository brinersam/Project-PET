using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.Core.Options;

namespace ProjectPet.AccountsModule.Infrastructure.Database;

public class AuthDbContext(IConfiguration configuration, IOptions<OptionsDb> options) : IdentityDbContext<User, Role, Guid>
{
    private readonly string DATABASE = configuration[options.Value.CStringKey]!;

    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<RefreshSession> RefreshSessions => Set<RefreshSession>();

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

        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

        modelBuilder.Entity<Role>()
            .ToTable("roles");

        modelBuilder.Entity<IdentityUserClaim<Guid>>()
            .ToTable("claims");

        modelBuilder.Entity<IdentityUserToken<Guid>>()
            .ToTable("user-tokens");

        modelBuilder.Entity<IdentityUserLogin<Guid>>()
            .ToTable("user-logins");

        modelBuilder.Entity<IdentityRoleClaim<Guid>>()
            .ToTable("claims-roles");

        modelBuilder.Entity<IdentityUserRole<Guid>>()
            .ToTable("user-roles");

        modelBuilder.HasDefaultSchema("auth"); 
    }

    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => builder.AddConsole());
    }
}
