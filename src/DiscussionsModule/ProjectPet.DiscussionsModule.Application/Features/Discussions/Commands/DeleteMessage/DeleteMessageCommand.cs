namespace ProjectPet.DiscussionsModule.Application.Features.Discussions.Commands.DeleteMessage;
public record DeleteMessageCommand(
    Guid UserId,
    Guid DiscussionId,
    Guid MessageId)
{ }
