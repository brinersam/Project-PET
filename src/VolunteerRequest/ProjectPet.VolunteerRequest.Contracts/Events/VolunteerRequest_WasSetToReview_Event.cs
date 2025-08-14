using MediatR;
using System.Runtime.Serialization;

namespace ProjectPet.VolunteerRequests.Contracts.Events;


[Serializable]
public class DomainEventException : Exception
{
    public DomainEventException(string message)
        : base(message)
    { }

    protected DomainEventException(SerializationInfo info, StreamingContext ctxt)
        : base(info, ctxt)
    { }
}

public interface IDomainEvent : INotification;
public record VolunteerRequest_WasSetToReview_Event(
    Guid VolunteerRequestId,
    Guid[] UserIds)
    : IDomainEvent { }
