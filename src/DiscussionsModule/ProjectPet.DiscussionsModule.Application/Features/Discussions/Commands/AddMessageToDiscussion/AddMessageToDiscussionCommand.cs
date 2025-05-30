using ProjectPet.Core.Requests;
using ProjectPet.DiscussionsModule.Contracts.Requests;

namespace ProjectPet.DiscussionsModule.Application.Features.Discussions.Commands.AddMessageToDiscussion;
public record AddMessageToDiscussionCommand(
    Guid DiscussionId,
    Guid UserId,
    string MessageBody) : IMapFromRequest<AddMessageToDiscussionCommand, AddMessageToDiscussionRequest, Guid, Guid>
{
    public static AddMessageToDiscussionCommand FromRequest(AddMessageToDiscussionRequest request, Guid discussionId, Guid userId)
        => new(discussionId, userId, request.MessageBody);
}
