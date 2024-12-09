using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProjectPet.AccountsModule.Application.Services;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.AccountsModule.Domain.Accounts;
using ProjectPet.AccountsModule.Infrastructure.Database;
using ProjectPet.AccountsModule.Infrastructure.Options;
using ProjectPet.AccountsModule.Infrastructure.Seeding.SeedDtos;
using System.Text.Json;

namespace ProjectPet.AccountsModule.Infrastructure.Seeding;
public class DatabaseAccountsSeeder
{
    private readonly AuthDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<DatabaseAccountsSeeder> _logger;
    private readonly UserFactoryService _userFactoryService;
    private readonly AdminCredsOptions _adminCreds;
    private bool _verboseLogging;

    public DatabaseAccountsSeeder(
        AuthDbContext dbContext,
        UserManager<User> userManager,
        UserFactoryService userFactoryService,
        IOptions<AdminCredsOptions> adminCreds,
        ILogger<DatabaseAccountsSeeder> logger)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _userFactoryService = userFactoryService;
        _adminCreds = adminCreds.Value;
        _logger = logger;
    }

    public async Task SeedAsync(
        bool verboseLogging = false,
        CancellationToken cancellationToken = default)
    {
        _verboseLogging = verboseLogging;
        _logger.LogInformation("Seeding database...");

        await SeedRolesAsync(cancellationToken);
        await SeedPermissionsAsync(cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await AddPermissionRoleRelationsAsync(cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await AddAdminUserAsync(cancellationToken);

        _logger.LogInformation("Database seeded!...");
    }

    private async Task SeedRolesAsync(CancellationToken cancellationToken = default)
    {
        var path = "Json/Seeding/permissions.json";
        var existingPermissionCodes = _dbContext.Permissions.Select(x => x.Code).ToHashSet();
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
            await _dbContext.Permissions.AddAsync(new Permission() { Code = permCode }, cancellationToken);
        }
    }

    private async Task SeedPermissionsAsync(CancellationToken cancellationToken = default)
    {
        var path = "Json/Seeding/roles.json";
        var existingRoles = _dbContext.Roles.Select(x => x.NormalizedName).ToHashSet();
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
            await _dbContext.Roles.AddAsync(
                new Role()
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpper()
                },
                cancellationToken);
        }
    }
    private async Task AddPermissionRoleRelationsAsync(CancellationToken cancellationToken = default)
    {
        var path = "Json/Seeding/rolepermissions.json";

        var seededRolePermissionsJson = await File.ReadAllTextAsync(path, cancellationToken);
        var seededRolePermissions = JsonSerializer.Deserialize<RolePermissionsSets>(seededRolePermissionsJson, JsonSerializerOptions.Default);

        if (seededRolePermissions is null)
        {
            _logger.LogWarning($"Could not deserialise {path}");
            return;
        }

        await AddRoleRelationsAsync("Admin", seededRolePermissions.Admin, cancellationToken);
        await AddRoleRelationsAsync("Member", seededRolePermissions.Member, cancellationToken);
        await AddRoleRelationsAsync("Volunteer", seededRolePermissions.Volunteer, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    private async Task AddAdminUserAsync(CancellationToken cancellationToken = default)
    {
        var adminExists = await _userManager.FindByEmailAsync(_adminCreds.EMAIL) is not null;
        if (adminExists)
            return;

        var createAdminResult = await _userFactoryService.CreateAdminUserAsync(
            _adminCreds.USERNAME,
            _adminCreds.PASSWORD,
            _adminCreds.EMAIL,
            new AdminAccount("Admin"));

        if (createAdminResult.IsFailure)
            throw new Exception(createAdminResult.Error[0].Message);

        _logger.LogInformation($"Successfully added admin role from .env settings!");
    }

    private async Task AddRoleRelationsAsync(
        string roleName,
        HashSet<string> permissionsCodesToSeed,
        CancellationToken cancellationToken = default)
    {
        var role = await _dbContext.Roles.FirstOrDefaultAsync(x => String.Equals(x.Name, roleName), cancellationToken);
        if (role is null)
        {
            _logger.LogWarning($"Could not retrieve role {roleName} for seeding relations!");
            return;
        }

        var existingRolePermissionCodes = _dbContext.RolePermissions
            .Where(rp => rp.RoleId == role.Id && permissionsCodesToSeed.Contains(rp.Permission.Code))
            .Select(rp => rp.Permission.Code)
            .ToHashSet();

        var permissionsToRelate = await _dbContext.Permissions
            .Where(p => existingRolePermissionCodes.Contains(p.Code) == false && permissionsCodesToSeed.Contains(p.Code))
            .ToListAsync(cancellationToken);

        int permsToSeed = permissionsCodesToSeed.Count - existingRolePermissionCodes.Count;

        _logger.LogInformation($"Database has to seed {permsToSeed}/{permissionsCodesToSeed.Count} permission relations for role {roleName}...");

        foreach (var perm in permissionsToRelate)
        {
            var rp = new RolePermission() { PermissionId = perm.Id, RoleId = role.Id };
            await _dbContext.RolePermissions.AddAsync(rp, cancellationToken);
            permsToSeed--;
            if (_verboseLogging) _logger.LogInformation($"Added permission for role: {role.Name} --> {perm.Code}");
        }

        if (permsToSeed > 0)
            _logger.LogWarning($"Not every permission relation was seeded for role {role.Name}!");
    }
}

