using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProjectPet.Core.Abstractions;
using ProjectPet.Core.Options;
using ProjectPet.SharedKernel.Entities.AbstractBase;
using ProjectPet.VolunteerModule.Domain.Models;
using ProjectPet.VolunteerModule.Infrastructure.Database;

namespace ProjectPet.VolunteerModule.Infrastructure.BackgroundServices;
public class SoftDeleteCleanupService : BackgroundService
{
    private readonly ILogger<SoftDeleteCleanupService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly OptionsDb _options;

    public SoftDeleteCleanupService(
        ILogger<SoftDeleteCleanupService> logger,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<OptionsDb> options)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int cleanupFrequencyHours = 1000 * 60 * _options.SoftDeletedCleanupFrequencyHours;

        _logger.LogInformation("{serviceName} was started!", nameof(SoftDeleteCleanupService));

        while (stoppingToken.IsCancellationRequested == false)
        {
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            await Task.Delay(cleanupFrequencyHours, stoppingToken);

            _logger.LogInformation("{serviceName} is working...", nameof(SoftDeleteCleanupService));

            var transaction = await unitOfWork.BeginTransactionAsync(stoppingToken);
            int cleanedUpEntities = await RemoveExpiredSoftDeletedEntitiesFromSetAsync<Volunteer>(dbContext);
            cleanedUpEntities += await RemoveExpiredSoftDeletedEntitiesFromSetAsync<Pet>(dbContext);
            await unitOfWork.SaveChangesAsync(stoppingToken);
            transaction.Commit();

            _logger.LogInformation(
                "{serviceName} cleaned up {x} entires!",
                nameof(SoftDeleteCleanupService),
                cleanedUpEntities);
        }

        await Task.CompletedTask;
    }

    private async Task<int> RemoveExpiredSoftDeletedEntitiesFromSetAsync<T>(DbContext context) where T : SoftDeletableEntity
    {
        int cleanedUpEntities = 0;
        var set = await context
            .Set<T>()
            .Where(x => x.IsDeleted == true)
            .ToListAsync();

        foreach (T entity in set)
        {
            var lifetimeDays = (DateTime.UtcNow - entity.DeletionDate).Days;
            if (lifetimeDays < _options.SoftDeletedMaxLifeTimeDays)
                continue;

            cleanedUpEntities++;
            set.Remove(entity);
        }

        return cleanedUpEntities;
    }
}
