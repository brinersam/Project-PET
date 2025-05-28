using CSharpFunctionalExtensions;
using ProjectPet.DiscussionsModule.Application.Interfaces;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.DiscussionsModule.Application.Features.Discussions.Commands.AddMessageToDiscussion;
public class AddMessageToDiscussionHandler
{
    private readonly IDiscussionsRepository _discussionRepository;

    public AddMessageToDiscussionHandler(
        IDiscussionsRepository requestRepository)
    {
        _discussionRepository = requestRepository;
    }

    public async Task<Result<Guid, Error>> HandleAsync(
        AddMessageToDiscussionCommand command,
        CancellationToken cancellationToken)
    {
        var getRes = await _discussionRepository.GetByIdAsync(command.DiscussionId, cancellationToken);
        if (getRes.IsFailure)
            return getRes.Error;

        var addCommentRes = getRes.Value.AddComment(command.UserId, command.MessageBody);
        if (addCommentRes.IsFailure)
            return addCommentRes.Error;

        var saveRes = await _discussionRepository.Save(getRes.Value, cancellationToken);
        if (saveRes.IsFailure)
            return saveRes.Error;

        return getRes.Value.Id;
    }
}
