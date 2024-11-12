using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProjectPet.Application.Providers;
using ProjectPet.Infrastructure.MessageQueues;

namespace ProjectPet.Infrastructure.BackgroundServices;
public class FileCleanupBackgroundService : BackgroundService
{
    private readonly ILogger<FileCleanupBackgroundService> _logger;
    private readonly IMessageQueue<IEnumerable<FileDataDto>> _messageQueue;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public FileCleanupBackgroundService(
        ILogger<FileCleanupBackgroundService> logger,
        IMessageQueue<IEnumerable<FileDataDto>> messageQueue,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _messageQueue = messageQueue;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var fileProvider = scope.ServiceProvider.GetRequiredService<IFileProvider>();

        _logger.LogInformation($"{nameof(FileCleanupBackgroundService)} service was started!");

        while (stoppingToken.IsCancellationRequested == false)
        {
            var fileDatas = await _messageQueue.ReadAsync(stoppingToken);

            foreach (var fileData in fileDatas)
            {
                var result = await fileProvider.DeleteFilesAsync(
                    fileData.Bucket,
                    fileData.UserId,
                    [fileData.ObjectName],
                    stoppingToken);

                if (result.IsFailure)
                {
                    _logger.LogError($"Failed to clean up scheduled file {fileData.ObjectName}! Error: {result.Error}");
                    continue;
                }

                _logger.LogInformation($"Cleaned up scheduled file {fileData.ObjectName}!");
            }

            await Task.Delay(1000 * 60 * 60, stoppingToken);
        }

        await Task.CompletedTask;
    }
}
