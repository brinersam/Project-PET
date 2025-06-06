﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjectPet.Core.Options;
using ProjectPet.VolunteerRequests.Domain.Models;

namespace ProjectPet.VolunteerRequests.Infrastructure.Database;

public class WriteDbContext(IConfiguration configuration) : DbContext
{
    private readonly string DATABASE = configuration[configuration.GetRequiredSection(OptionsDb.SECTION).Get<OptionsDb>()!.CStringKey];
    public DbSet<VolunteerRequest> VolunteerRequests => Set<VolunteerRequest>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(DATABASE);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(WriteDbContext).Assembly,
            x => x.FullName!.Contains("Configurations.Write"));

        modelBuilder.HasDefaultSchema("volunteer-requests");
    }

    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => builder.AddConsole());
    }
}
