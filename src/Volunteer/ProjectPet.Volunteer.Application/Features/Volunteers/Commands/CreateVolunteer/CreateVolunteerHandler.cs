﻿using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerModule.Application.Interfaces;
using ProjectPet.VolunteerModule.Domain.Models;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.CreateVolunteer;

public class CreateVolunteerHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<CreateVolunteerHandler> _logger;

    public CreateVolunteerHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<CreateVolunteerHandler> logger)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> HandleAsync(
        CreateVolunteerCommand cmd,
        CancellationToken cancellationToken = default)
    {
        var phoneNumberRes = Phonenumber.Create(
            cmd.VolunteerDto.Phonenumber.Phonenumber,
            cmd.VolunteerDto.Phonenumber.PhonenumberAreaCode).Value;

        var volunteerRes = Volunteer.Create(
            Guid.NewGuid(),
            cmd.VolunteerDto.FullName,
            cmd.VolunteerDto.Email,
            cmd.VolunteerDto.Description,
            cmd.VolunteerDto.YOExperience,
            phoneNumberRes);

        if (volunteerRes.IsFailure)
            return volunteerRes.Error;

        var addRes = await _volunteerRepository.AddAsync
                                (volunteerRes.Value, cancellationToken);

        if (addRes.IsFailure)
            return addRes.Error;

        _logger.LogInformation("Created volunteer {name} with id {id}", volunteerRes.Value.FullName, volunteerRes.Value.Id);

        return volunteerRes.Value.Id;
    }
}
