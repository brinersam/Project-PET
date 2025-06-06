﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectPet.Core.Database;
using ProjectPet.Core.Requests;
using ProjectPet.VolunteerModule.Application.Interfaces;
using ProjectPet.VolunteerModule.Infrastructure.BackgroundServices;
using ProjectPet.VolunteerModule.Infrastructure.Database;
using ProjectPet.VolunteerModule.Infrastructure.Repositories;

namespace ProjectPet.VolunteerModule.Infrastructure;
public static class DependencyInjection
{
    public static IHostApplicationBuilder AddVolunteerModuleInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IReadDbContext, ReadDbContext>();

        builder.Services.AddScoped<WriteDbContext>();

        builder.Services.AddScoped<IVolunteerRepository, VolunteerRepository>();

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        builder.Services.AddHostedService<SoftDeleteCleanupService>();

        return builder;
    }
}
