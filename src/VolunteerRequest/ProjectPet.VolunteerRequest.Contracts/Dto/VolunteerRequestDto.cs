using ProjectPet.SharedKernel.SharedDto;

namespace ProjectPet.VolunteerRequests.Contracts.Dto;
public class VolunteerRequestDto
{
    public Guid AdminId { get; init; }
    public Guid UserId { get; init; }
    public Guid DiscussionId { get; init; }
    public VolunteerAccountDto VolunteerData { get; init; } = null!;
    public VolunteerRequestStatusDto Status { get; init; }
    public DateTime CreatedAt { get; init; }
    public string RejectionComment { get; init; } = string.Empty;
}

