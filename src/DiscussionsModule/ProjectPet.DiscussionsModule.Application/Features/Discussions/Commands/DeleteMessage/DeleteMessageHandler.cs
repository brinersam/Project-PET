using CSharpFunctionalExtensions;
using ProjectPet.DiscussionsModule.Application.Interfaces;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.DiscussionsModule.Application.Features.Discussions.Commands.DeleteMessage;
public class DeleteMessageHandler
{
    private readonly IDiscussionsRepository _discussionRepository;

    public DeleteMessageHandler(
        IDiscussionsRepository requestRepository)
    {
        _discussionRepository = requestRepository;
    }

    public async Task<Result<Guid, Error>> HandleAsync(
        DeleteMessageCommand command,
        CancellationToken cancellationToken)
    {
        var getRes = await _discussionRepository.GetByIdAsync(command.DiscussionId, cancellationToken);
        if (getRes.IsFailure)
            return getRes.Error;

        var deleyeCommentRes = getRes.Value.DeleteComment(
            command.UserId,
            command.MessageId);
        if (deleyeCommentRes.IsFailure)
            return deleyeCommentRes.Error;

        var saveRes = await _discussionRepository.Save(getRes.Value, cancellationToken);
        if (saveRes.IsFailure)
            return saveRes.Error;

        return getRes.Value.Id;
    }
}