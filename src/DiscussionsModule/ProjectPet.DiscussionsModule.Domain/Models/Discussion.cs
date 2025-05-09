using CSharpFunctionalExtensions;
using ProjectPet.Core.Validator;
using ProjectPet.SharedKernel.Entities.AbstractBase;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.DiscussionsModule.Domain.Models;
public class Discussion : EntityBase
{
    public Guid RelatedEntityId { get; private set; }
    public List<Guid> _userIds { get; private set; }
    public List<Message> _messages { get; private set; }
    public bool IsClosed { get; private set; }

    public IReadOnlyList<Guid> UserIds => _userIds.AsReadOnly();
    public IReadOnlyList<Message> Messages => _messages.AsReadOnly();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public Discussion() : base(Guid.Empty) {}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    private Discussion(Guid relatedEntityId,
                       IEnumerable<Guid> userIds,
                       bool isClosed) : base(Guid.Empty)
    {
        RelatedEntityId = relatedEntityId;
        _userIds = userIds.ToList();
        _messages = [];
        IsClosed = isClosed;
    }

    public static Result<Discussion, Error> Create(Guid relatedEntityId,
                                                   IEnumerable<Guid> userIds)
    {
        var guidValidator = Validator.ValidatorNull<Guid>();
        var valresGuid = guidValidator.Check(relatedEntityId, nameof(relatedEntityId));
        if (valresGuid.IsFailure)
            return valresGuid.Error;

        if (userIds.Count() < 2)
            return Error.Failure("user.count.invalid", "Need at least two participants to start a discussion!");

        return new Discussion(relatedEntityId,
                              userIds,
                              isClosed: false);
    }

    public UnitResult<Error> AddComment(Guid userId, string text)
    {
        if (IsClosedDiscussion(out Error error))
            return error;

        if (_userIds.Contains(userId) == false)
            return Error.Failure("not.a.participant", "User needs to be a participant of the discussion in order to leave comments");

        var msgCreation = Message.Create(userId, text);
        if (msgCreation.IsFailure)
            return msgCreation.Error;

        _messages.Add(msgCreation.Value);
        return Result.Success<Error>();
    }

    public UnitResult<Error> DeleteComment(Guid userId, Guid commentId)
    {
        if (IsClosedDiscussion(out Error error))
            return error;

        Message msg = _messages.FirstOrDefault(x => x.Id == commentId)!;
        if (msg is null)
            return Result.Success<Error>();

        if (msg.UserId != userId)
            return Error.Failure("not.author", "Not allowed");

        _messages.Remove(msg);

        return Result.Success<Error>();
    }

    public UnitResult<Error> EditComment(Guid userId, Guid commentId, string text)
    {
        if (IsClosedDiscussion(out Error error))
            return error;

        Message msg = _messages.FirstOrDefault(x => x.Id == commentId)!;
        if (msg is null)
            return Errors.General.NotFound(typeof(Message));

        if (msg.UserId != userId)
            return Error.Failure("not.author", "Not allowed");

        msg.Update(text);
        return Result.Success<Error>();
    }

    public UnitResult<Error> EndDiscussion()
    {
        IsClosed = true;
        return Result.Success<Error>();
    }

    private bool IsClosedDiscussion(out Error error)
    {
        error = IsClosed ? Error.Failure("discussion.closed", "Not allowed") : null!;
        return IsClosed;
    }

}
