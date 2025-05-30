using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Minio;
using ProjectPet.Core.Options;
using ProjectPet.FileManagement.Infrastructure;
using ProjectPet.FileManagement.Infrastructure.BackgroundServices;
using ProjectPet.FileManagement.Infrastructure.MessageQueues;
using ProjectPet.FileManagement.Infrastructure.Providers;
using ProjectPet.Core.Files;
using ProjectPet.Core.MessageQueues;

namespace ProjectPet.FileManagement.Infrastructure;
public static class DependencyInjection
{
    public static IHostApplicationBuilder AddFileManagementInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.AddMinio();
        builder.Services.AddHostedService<FileCleanupBackgroundService>();
        builder.Services.AddSingleton<IMessageQueue<IEnumerable<FileDataDto>>, FileInfoMessageQueue>();

        return builder;
    }
    private static IHostApplicationBuilder AddMinio(
        this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<OptionsMinIO>(
            builder.Configuration.GetSection(OptionsMinIO.SECTION));

        builder.Services.AddMinio(options =>
        {
            var config = builder.Configuration.GetSection(OptionsMinIO.SECTION).Get<OptionsMinIO>() ??
                throw new ArgumentNullException("Minio options not defined!");

            options.WithEndpoint(config.Endpoint);
            options.WithCredentials(config.AccessKey, config.SecretKey);
            options.WithSSL(config.WithSSL);
        });

        builder.Services.AddScoped<IFileProvider, MinioProvider>();

        return builder;
    }
}
