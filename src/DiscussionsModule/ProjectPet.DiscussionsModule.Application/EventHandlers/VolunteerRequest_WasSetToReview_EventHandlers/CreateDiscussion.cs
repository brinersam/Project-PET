using MediatR;
using ProjectPet.DiscussionsModule.Application.Interfaces;
using ProjectPet.DiscussionsModule.Domain.Models;
using ProjectPet.SharedKernel.Exceptions;
using ProjectPet.VolunteerRequests.Contracts.Events;

namespace ProjectPet.DiscussionsModule.Application.EventHandlers.VolunteerRequest_WasSetToReview_EventHandlers;
public class CreateDiscussion : INotificationHandler<VolunteerRequest_WasSetToReview_Event>
{
    private readonly IDiscussionsRepository _repository;

    public CreateDiscussion(IDiscussionsRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(VolunteerRequest_WasSetToReview_Event notification, CancellationToken cancellationToken)
    {
        var createRes = Discussion.Create(
            notification.VolunteerRequestId,
            notification.UserIds);

        if (createRes.IsFailure)
            throw new DomainEventException(createRes.Error.Message);

        var saveRes = await _repository.Save(createRes.Value, cancellationToken);
        if (saveRes.IsFailure)
            throw new DomainEventException(saveRes.Error.Message);
    }
}
