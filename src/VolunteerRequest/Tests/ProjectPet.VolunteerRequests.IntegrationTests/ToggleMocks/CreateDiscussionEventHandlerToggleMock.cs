using MediatR;
using ProjectPet.DiscussionsModule.Application.EventHandlers.VolunteerRequest_WasSetToReview_EventHandlers;
using ProjectPet.VolunteerRequests.Contracts.Events;

namespace ProjectPet.VolunteerRequests.IntegrationTests.ToggleMocks;
public class CreateDiscussionEventHandlerToggleMock : ToggleMockBase<INotificationHandler<VolunteerRequest_WasSetToReview_Event>, CreateDiscussion>, INotificationHandler<VolunteerRequest_WasSetToReview_Event>
{

    public Task Handle(VolunteerRequest_WasSetToReview_Event notification, CancellationToken cancellationToken)
    {
        if (IsMocked(nameof(Handle)))
            return Mock.Handle(notification, cancellationToken);
        else
            return CreateInstance().Handle(notification, cancellationToken);
    }
}
