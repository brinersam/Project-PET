using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.AccountsModule.Contracts;
using ProjectPet.Core.Database;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SharedKernel.SharedDto;
using ProjectPet.VolunteerRequests.Application.Interfaces;

namespace ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.Approve;
public class ApproveHandler
{
    private readonly IVolunteerRequestRepository _requestRepository;
    private readonly IAccountsModuleContract _accountsModule;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ApproveHandler> _logger;

    public ApproveHandler(
        IVolunteerRequestRepository requestRepository,
        IAccountsModuleContract accountsModule,
        IUnitOfWork unitOfWork,
        ILogger<ApproveHandler> logger)
    {
        _requestRepository = requestRepository;
        _accountsModule = accountsModule;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<UnitResult<Error>> HandleAsync(
        ApproveCommand command,
        CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var requestRes = await _requestRepository.GetByIdAsync(command.RequestId, cancellationToken);
        if (requestRes.IsFailure)
            return requestRes.Error;

        var approveRes = requestRes.Value.ApproveRequest();
        if (approveRes.IsFailure)
            return approveRes.Error;

        var data = new VolunteerAccountDto()
        {
            Certifications = requestRes.Value.VolunteerData.Certifications,
            Experience = requestRes.Value.VolunteerData.Experience,
            PaymentInfos = requestRes.Value.VolunteerData.PaymentInfos.Select(x => new PaymentInfoDto(x.Title, x.Instructions)).ToList()
        };

        var addDataToUserRes = await _accountsModule.MakeUserVolunteerAsync(
            requestRes.Value.UserId,
            data,
            cancellationToken);

        if (addDataToUserRes.IsFailure)
            return addDataToUserRes.Error;

        var saveRes = await _requestRepository.Save(requestRes.Value, cancellationToken);
        if (saveRes.IsFailure)
            return saveRes.Error;

        transaction.Commit();

        _logger.LogInformation(
            "Volunteer request (id {O1}) was approved. Was assigned admin (id {O2})",
            requestRes.Value.Id,
            requestRes.Value.AdminId);

        return Result.Success<Error>();
    }
}