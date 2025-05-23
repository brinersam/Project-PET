﻿using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.AccountsModule.Contracts;
using ProjectPet.AccountsModule.Contracts.Dto;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerRequests.Application.Interfaces;

namespace ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.Reject;
public class RejectHandler
{
    private readonly IVolunteerRequestRepository _requestRepository;
    private readonly IAccountsModuleContract _accountsModule;
    private readonly ILogger<RejectHandler> _logger;

    public RejectHandler(
        IVolunteerRequestRepository requestRepository,
        IAccountsModuleContract accountsModule,
        ILogger<RejectHandler> logger)
    {
        _requestRepository = requestRepository;
        _accountsModule = accountsModule;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> HandleAsync(
        RejectCommand command,
        CancellationToken cancellationToken)
    {
        var requestRes = await _requestRepository.GetByIdAsync(command.RequestId, cancellationToken);
        if (requestRes.IsFailure)
            return requestRes.Error;

        var requestRevisionRes = requestRes.Value.RejectRequest(command.RejectionComment);
        if (requestRevisionRes.IsFailure)
            return requestRevisionRes.Error;

        var permissionModifier = new PermissionModifierDto()
        {
            Code = command.PermissionCode,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsAllowed = false
        };

        var preventUserFromPostingRes = await _accountsModule.CreatePermissionModifierAsync(
            requestRes.Value.UserId,
            permissionModifier,
            cancellationToken);

        if (preventUserFromPostingRes.IsFailure)
            return preventUserFromPostingRes.Error;

        var saveRes = await _requestRepository.Save(requestRes.Value, cancellationToken);
        if (saveRes.IsFailure)
            return saveRes.Error;

        _logger.LogInformation(
            "Volunteer request (id {O1}) was rejected by user (id {O2})",
            requestRes.Value.Id,
            requestRes.Value.UserId);

        return requestRes.Value.Id;
    }
}