using CSharpFunctionalExtensions;
using ProjectPet.Core.Validator;
using ProjectPet.SharedKernel.Entities.AbstractBase;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.AccountsModule.Domain;
public class PermissionModifier : EntityBase
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public bool IsAllowed { get; private set; }
    public DateTime ExpiresAt { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    protected PermissionModifier(Guid id) : base(id) { } // efcore
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    private PermissionModifier(Guid id,
                               Guid userId,
                               string code,
                               bool isAllowed,
                               DateTime expiresAt) : this(id)
    {
        UserId = userId;
        Code = code;
        IsAllowed = isAllowed;
        ExpiresAt = expiresAt;
    }

    public static Result<PermissionModifier, Error> Create(Guid userId,
                                                           string code,
                                                           bool isAllowed,
                                                           DateTime expiresAt)
    {
        if (expiresAt <= DateTime.UtcNow)
            return Errors.General.ValueIsInvalid(expiresAt, nameof(expiresAt));

        var strValidationResult = Validator.ValidatorString().Check(code, nameof(code));
        if (strValidationResult.IsFailure)
            return strValidationResult.Error;

        return new PermissionModifier(
            Guid.Empty,
            userId,
            code,
            isAllowed,
            expiresAt);
    }
}
