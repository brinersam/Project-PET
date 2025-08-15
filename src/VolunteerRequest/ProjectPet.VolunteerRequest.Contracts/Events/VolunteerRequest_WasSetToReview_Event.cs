using ProjectPet.SharedKernel.Entities;

namespace ProjectPet.VolunteerRequests.Contracts.Events;

public record VolunteerRequest_WasSetToReview_Event(
    Guid VolunteerRequestId,
    Guid[] UserIds)
    : IDomainEvent
{ }
