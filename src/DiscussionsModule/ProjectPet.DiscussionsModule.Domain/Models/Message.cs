using CSharpFunctionalExtensions;
using ProjectPet.Core.Validator;
using ProjectPet.SharedKernel;
using ProjectPet.SharedKernel.Entities.AbstractBase;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.DiscussionsModule.Domain.Models;
public class Message : EntityBase
{
    public Guid UserId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string Text { get; private set; }
    public bool IsEdited { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public Message() : base(Guid.Empty) {}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    private Message(Guid userId,
                    DateTime createdAt,
                    string text,
                    bool isEdited) : base(Guid.NewGuid())
    {
        UserId = userId;
        CreatedAt = createdAt;
        Text = text;
        IsEdited = isEdited;
    }

    public static Result<Message, Error> Create(Guid userId,
                                                string text)
    {
        var stringValidator = Validator.ValidatorString(Constants.STRING_LEN_MEDIUM);
        var valresStr = stringValidator.Check(text, nameof(text));
        if (valresStr.IsFailure)
            return valresStr.Error;

        var guidValidator = Validator.ValidatorNull<Guid>();
        var valresGuid = guidValidator.Check(userId, nameof(userId));
        if (valresGuid.IsFailure)
            return valresGuid.Error;

        return new Message(
            userId,
            DateTime.UtcNow,
            text,
            isEdited: false);
    }

    public UnitResult<Error> Update(string text)
    {
        var stringValidator = Validator.ValidatorString(Constants.STRING_LEN_MEDIUM);
        var valresStr = stringValidator.Check(text, nameof(text));
        if (valresStr.IsFailure)
            return valresStr.Error;

        Text = text;
        IsEdited = true;
        return Result.Success<Error>();
    }
}
