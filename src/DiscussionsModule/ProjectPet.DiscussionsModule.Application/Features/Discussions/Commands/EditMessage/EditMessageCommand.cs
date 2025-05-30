using ProjectPet.Core.Requests;
using ProjectPet.DiscussionsModule.Contracts.Requests;

namespace ProjectPet.DiscussionsModule.Application.Features.Discussions.Commands.EditMessage;
public record EditMessageCommand(
    Guid UserId,
    Guid DiscussionId,
    Guid MessageId,
    string MessageBody) : IMapFromRequest<EditMessageCommand, EditMessageRequest, Guid, Guid, Guid>
{
    public static EditMessageCommand FromRequest(EditMessageRequest request, Guid discussionId, Guid messageId, Guid userId)
        => new(userId, discussionId, messageId, request.MessageBody);
}
