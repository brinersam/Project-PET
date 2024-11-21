﻿using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.VolunteerModule.Domain.Models;
using ProjectPet.VolunteerModule.Application.Interfaces;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.CreateVolunteer;

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

        var paymentInfos = cmd.PaymentInfoDtos ?? [];
        var socialNetworks = cmd.SocialNetworkDtos ?? [];



        var volunteerRes = Volunteer.Create(
            Guid.NewGuid(),
            cmd.VolunteerDto.FullName,
            cmd.VolunteerDto.Email,
            cmd.VolunteerDto.Description,
            cmd.VolunteerDto.YOExperience,
            phoneNumberRes,
            paymentInfos.Select(x => PaymentInfo.Create(x.Title, x.Instructions).Value).ToList(),
            socialNetworks.Select(x => SocialNetwork.Create(x.Link, x.Name).Value).ToList());

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
