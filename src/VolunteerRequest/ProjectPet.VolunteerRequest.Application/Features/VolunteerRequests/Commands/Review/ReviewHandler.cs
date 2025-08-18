using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using ProjectPet.Core.Database;
using ProjectPet.Core.Extensions;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SharedKernel.Exceptions;
using ProjectPet.VolunteerRequests.Application.Interfaces;

namespace ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.Review;
public class ReviewHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteerRequestRepository _requestRepository;
    private readonly IPublisher _publisher;
    private readonly ILogger<ReviewHandler> _logger;

    public ReviewHandler(
        IUnitOfWork unitOfWork,
        IVolunteerRequestRepository requestRepository,
        IPublisher publisher,
        ILogger<ReviewHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _requestRepository = requestRepository;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> HandleAsync(
        ReviewCommand command,
        CancellationToken cancellationToken)
    {
        var requestRes = await _requestRepository.GetByIdAsync(command.RequestId, cancellationToken);
        if (requestRes.IsFailure)
            return requestRes.Error;
        var request = requestRes.Value;

        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var beginReviewRes = request.BeginReview(command.AdminId);
        if (beginReviewRes.IsFailure)
            return beginReviewRes.Error;

        try
        {
            await _publisher.PublishDomainEventsAsync(request, cancellationToken);
        }
        catch (DomainEventException ex)
        {
            return Error.Failure("domain.event.failure", ex.Message);
        }

        var saveRes = await _requestRepository.Save(requestRes.Value, cancellationToken);
        if (saveRes.IsFailure)
            return saveRes.Error;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        transaction.Commit();

        _logger.LogInformation(
            "Volunteer request (id {O1}) taken into review by user (id {O2})",
            request.Id,
            request.UserId);

        return saveRes.Value;
    }
}

