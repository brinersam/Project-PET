﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Minio;
using Minio.AspNetCore;
using ProjectPet.Application.Database;
using ProjectPet.Application.Providers;
using ProjectPet.Application.UseCases.AnimalSpecies;
using ProjectPet.Application.UseCases.Volunteers;
using ProjectPet.Infrastructure.BackgroundServices;
using ProjectPet.Infrastructure.MessageQueues;
using ProjectPet.Infrastructure.Options;
using ProjectPet.Infrastructure.Providers;
using ProjectPet.Infrastructure.Repositories;

namespace ProjectPet.Infrastructure;

public static class Inject
{
    public static IHostApplicationBuilder AddInfrastructure(
        this IHostApplicationBuilder builder)
    {
        builder.AddMinio();

        builder.Services.AddScoped<ApplicationDbContext>();
        builder.Services.AddScoped<IVolunteerRepository, VolunteerRepository>();
        builder.Services.AddScoped<ISpeciesRepository, SpeciesRepository>();

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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
