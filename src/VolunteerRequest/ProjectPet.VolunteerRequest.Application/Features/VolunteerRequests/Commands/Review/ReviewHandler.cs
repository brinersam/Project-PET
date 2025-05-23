using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.Core.Abstractions;
using ProjectPet.DiscussionsModule.Contracts;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerRequests.Application.Interfaces;

namespace ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.Review;
public class ReviewHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDiscussionModuleContract _discussionModule;
    private readonly IVolunteerRequestRepository _requestRepository;
    private readonly ILogger<ReviewHandler> _logger;

    public ReviewHandler(
        IUnitOfWork unitOfWork,
        IDiscussionModuleContract discussionModule,
        IVolunteerRequestRepository requestRepository,
        ILogger<ReviewHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _discussionModule = discussionModule;
        _requestRepository = requestRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> HandleAsync(
        ReviewCommand command,
        CancellationToken cancellationToken)
    {
        var requestRes = await _requestRepository.GetByIdAsync(command.RequestId, cancellationToken);
        if (requestRes.IsFailure)
            return requestRes.Error;

        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var discussionRes = await _discussionModule.CreateDiscussionAsync(
            requestRes.Value.Id,
            [requestRes.Value.UserId, command.AdminId]);

        if (discussionRes.IsFailure)
            return discussionRes.Error;

        var beginReviewRes = requestRes.Value.BeginReview(command.AdminId);
        if (beginReviewRes.IsFailure)
            return beginReviewRes.Error;

        var saveRes = await _requestRepository.Save(requestRes.Value, cancellationToken);
        if (saveRes.IsFailure)
            return saveRes.Error;

        transaction.Commit();

        _logger.LogInformation(
            "Volunteer request (id {O1}) taken into review by user (id {O2})",
            requestRes.Value.Id,
            requestRes.Value.UserId);

        return saveRes.Value;
    }
}
