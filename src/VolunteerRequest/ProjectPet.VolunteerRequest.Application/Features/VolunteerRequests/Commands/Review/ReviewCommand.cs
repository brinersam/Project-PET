namespace ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.Review;
public record ReviewCommand(
    Guid RequestId,
    Guid AdminId)
{ }
