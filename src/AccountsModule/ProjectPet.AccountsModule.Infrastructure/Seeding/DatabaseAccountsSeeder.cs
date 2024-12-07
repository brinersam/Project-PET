using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.AccountsModule.Infrastructure.Database;
using ProjectPet.AccountsModule.Infrastructure.Seeding.SeedDtos;
using System.Text.Json;

namespace ProjectPet.AccountsModule.Infrastructure.Seeding;
public class DatabaseAccountsSeeder
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<DatabaseAccountsSeeder> _logger;

    private bool _verboseLogging;

    public DatabaseAccountsSeeder(
        IServiceScopeFactory serviceFactory,
        ILogger<DatabaseAccountsSeeder> logger)
    {
        _serviceScopeFactory = serviceFactory;
        _logger = logger;
    }

    public async Task SeedAsync(
        bool verboseLogging = false,
        CancellationToken cancellationToken = default)
    {
        _verboseLogging = verboseLogging;
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();

        _logger.LogInformation("Seeding database...");

        await SeedRolesAsync(dbContext, cancellationToken);
        await SeedPermissionsAsync(dbContext, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        await AddPermissionRoleRelationsAsync(dbContext, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Database seeded!...");
    }

    private async Task SeedRolesAsync(AuthDbContext dbContext, CancellationToken cancellationToken = default)
    {
        var path = "Json/Seeding/permissions.json";
        var existingPermissionCodes = dbContext.Permissions.Select(x => x.Code).ToHashSet();
        var seededPermsJson = await File.ReadAllTextAsync(path, cancellationToken);
        var seededPermCodesSet = JsonSerializer.Deserialize<StringHashset>(seededPermsJson, JsonSerializerOptions.Default);

        if (seededPermCodesSet is null)
        {
            _logger.LogWarning($"Could not deserialise {path}");
            return;
        }

        var permsToSeed = seededPermCodesSet.Data.Where(permCode => existingPermissionCodes.Contains(permCode) == false);
        _logger.LogInformation($"Database has to seed {permsToSeed.Count()}/{seededPermCodesSet.Data.Count} permissions...");

        if (permsToSeed.Count() <= 0)
            return;

        foreach (var permCode in permsToSeed)
        {
            if (_verboseLogging) _logger.LogInformation($"Adding permission {permCode}...");
            await dbContext.Permissions.AddAsync(new Permission() { Code = permCode }, cancellationToken);
        }
    }

    private async Task SeedPermissionsAsync(AuthDbContext dbContext, CancellationToken cancellationToken = default)
    {
        var path = "Json/Seeding/roles.json";
        var existingRoles = dbContext.Roles.Select(x => x.NormalizedName).ToHashSet();
        var seededRolesJson = await File.ReadAllTextAsync(path, cancellationToken);
        var seededRoleNamesSet = JsonSerializer.Deserialize<StringHashset>(seededRolesJson, JsonSerializerOptions.Default);

        if (seededRoleNamesSet is null)
        {
            _logger.LogWarning($"Could not deserialise {path}");
            return;
        }

        var rolesToSeed = seededRoleNamesSet.Data.Where(roleName => existingRoles.Contains(roleName.ToUpper()) == false);
        _logger.LogInformation($"Database has to seed {rolesToSeed.Count()}/{seededRoleNamesSet.Data.Count} roles...");

        if (rolesToSeed.Count() <= 0)
            return;

        foreach (var roleName in rolesToSeed)
        {
            if (_verboseLogging) _logger.LogInformation($"Adding role {roleName}...");
            await dbContext.Roles.AddAsync(
                new Role()
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpper()
                },
                cancellationToken);
        }
    }
    private async Task AddPermissionRoleRelationsAsync(AuthDbContext dbContext, CancellationToken cancellationToken = default)
    {
        var path = "Json/Seeding/rolepermissions.json";

        var seededRolePermissionsJson = await File.ReadAllTextAsync(path, cancellationToken);
        var seededRolePermissions = JsonSerializer.Deserialize<RolePermissionsSets>(seededRolePermissionsJson, JsonSerializerOptions.Default);

        if (seededRolePermissions is null)
        {
            _logger.LogWarning($"Could not deserialise {path}");
            return;
        }

        await AddRoleRelationsAsync(dbContext, "Admin", seededRolePermissions.Admin, cancellationToken);
        await AddRoleRelationsAsync(dbContext, "Member", seededRolePermissions.Member, cancellationToken);
        await AddRoleRelationsAsync(dbContext, "Volunteer", seededRolePermissions.Volunteer, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }


    private async Task AddRoleRelationsAsync(
        AuthDbContext dbContext,
        string roleName,
        HashSet<string> permissionsCodesToSeed,
        CancellationToken cancellationToken = default)
    {
        var role = await dbContext.Roles.FirstOrDefaultAsync(x => String.Equals(x.Name, roleName), cancellationToken);
        if (role is null)
        {
            _logger.LogWarning($"Could not retrieve role {roleName} for seeding relations!");
            return;
        }

        var existingRolePermissionCodes = dbContext.RolePermissions
            .Where(rp => rp.RoleId == role.Id && permissionsCodesToSeed.Contains(rp.Permission.Code))
            .Select(rp => rp.Permission.Code)
            .ToHashSet();

        var permissionsToRelate = await dbContext.Permissions
            .Where(p => existingRolePermissionCodes.Contains(p.Code) == false && permissionsCodesToSeed.Contains(p.Code))
            .ToListAsync(cancellationToken);

        int permsToSeed = permissionsCodesToSeed.Count - existingRolePermissionCodes.Count;

        _logger.LogInformation($"Database has to seed {permsToSeed}/{permissionsCodesToSeed.Count} permission relations for role {roleName}...");

        foreach (var perm in permissionsToRelate)
        {
            var rp = new RolePermission() { PermissionId = perm.Id, RoleId = role.Id };
            await dbContext.RolePermissions.AddAsync(rp, cancellationToken);
            permsToSeed--;
            if (_verboseLogging) _logger.LogInformation($"Added permission for role: {role.Name} --> {perm.Code}");
        }

        if (permsToSeed > 0)
            _logger.LogWarning($"Not every permission relation was seeded for role {role.Name}!");
    }
}

