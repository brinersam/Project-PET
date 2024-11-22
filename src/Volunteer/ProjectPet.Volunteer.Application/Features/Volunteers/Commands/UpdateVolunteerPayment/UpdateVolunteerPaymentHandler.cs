using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerModule.Application.Interfaces;
using ProjectPet.VolunteerModule.Domain.Models;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.UpdateVolunteerPayment;

public class UpdateVolunteerPaymentHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<UpdateVolunteerPaymentHandler> _logger;

    public UpdateVolunteerPaymentHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<UpdateVolunteerPaymentHandler> logger)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
    }
    public async Task<Result<Guid, Error>> HandleAsync(
        UpdateVolunteerPaymentCommand request,
        CancellationToken cancellationToken = default)
    {
        var volunteerRes = await _volunteerRepository.GetByIdAsync(request.Id, cancellationToken);
        if (volunteerRes.IsFailure)
            return volunteerRes.Error;

        var PaymentMethodsList = request.PaymentInfos
                                    .Select(x => PaymentInfo.Create(x.Title, x.Instructions).Value)
                                    .ToList();

        volunteerRes.Value.UpdatePaymentMethods(PaymentMethodsList);

        var saveRes = await _volunteerRepository.Save(volunteerRes.Value, cancellationToken);
        if (saveRes.IsFailure)
            return saveRes.Error;

        _logger.LogInformation("Updated volunteer with id {id} successfully!", saveRes.Value);
        return saveRes.Value;
    }
}
