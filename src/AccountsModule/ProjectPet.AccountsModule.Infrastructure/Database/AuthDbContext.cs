using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjectPet.AccountsModule.Application.Models;
using ProjectPet.Core.Options;

namespace ProjectPet.AccountsModule.Infrastructure.Database;

public class AuthDbContext(IConfiguration configuration) : IdentityDbContext<User, Role, Guid>
{
    private readonly string DATABASE = configuration[configuration.GetSection(OptionsDb.SECTION).Get<OptionsDb>()!.CString];

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

        modelBuilder.Entity<User>()
            .ToTable("users");

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
    }

    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => builder.AddConsole());
    }
}
