using CSharpFunctionalExtensions;
using ProjectPet.Core.Validator;
using ProjectPet.SharedKernel;
using ProjectPet.SharedKernel.Entities.AbstractBase;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.VolunteerRequests.Domain.Models;
public class VolunteerRequest : EntityBase
{
    public Guid AdminId { get; private set; }
    public Guid UserId { get; private set; }
    public Guid DiscussionId { get; private set; }
    public VolunteerAccountData VolunteerData { get; private set; }
    public VolunteerRequestStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string RejectionComment { get; private set; }

#pragma warning disable CS8618 // efcore
    protected VolunteerRequest(Guid id) : base(id)
    { }
#pragma warning restore CS8618

    private VolunteerRequest(
        Guid id,
        Guid adminId,
        Guid userId,
        Guid discussionId,
        VolunteerAccountData volunteerData,
        VolunteerRequestStatus requestStatus,
        DateTime createdAt,
        string rejectionComment) : base(id)
    {
        AdminId = adminId;
        UserId = userId;
        DiscussionId = discussionId;
        VolunteerData = volunteerData;
        Status = requestStatus;
        CreatedAt = createdAt;
        RejectionComment = rejectionComment;
    }

    public static Result<VolunteerRequest, Error> Create(
        Guid userId,
        Guid discussionId,
        VolunteerAccountData volunteerData)
    {
        return new VolunteerRequest(
            Guid.Empty,
            Guid.Empty,
            userId,
            discussionId,
            volunteerData,
            VolunteerRequestStatus.submitted,
            DateTime.UtcNow,
            string.Empty);
    }

    public UnitResult<Error> UpdateVolunteerData(VolunteerAccountData account)
    {
        if (TryTransitionTo(VolunteerRequestStatus.onReview, out Error error) == false)
            return error;

        VolunteerData = account;
        return Result.Success<Error>();
    }

    public UnitResult<Error> BeginReview(Guid adminId)
    {
        if (TryTransitionTo(VolunteerRequestStatus.onReview, out Error error) == false)
            return error;

        AdminId = adminId;
        return Result.Success<Error>();
    }

    public UnitResult<Error> RequestRevision(string rejectionComment)
    {
        var validatorStr = Validator.ValidatorString(Constants.STRING_LEN_MEDIUM);
        var result = validatorStr.Check(rejectionComment, nameof(rejectionComment));
        if (result.IsFailure)
            return result.Error;

        if (TryTransitionTo(VolunteerRequestStatus.revisionRequired, out Error error) == false)
            return error;
        RejectionComment = rejectionComment;

        return Result.Success<Error>();
    }

    public UnitResult<Error> RejectRequest(string rejectionComment = "")
    {
        if (TryTransitionTo(VolunteerRequestStatus.rejected, out Error error) == false)
            return error;
        RejectionComment = rejectionComment;

        return Result.Success<Error>();
    }

    public UnitResult<Error> ApproveRequest()
    {
        if (TryTransitionTo(VolunteerRequestStatus.approved, out Error error) == false)
            return error;

        return Result.Success<Error>();
    }

    private bool TryTransitionTo(VolunteerRequestStatus goal, out Error Error)
    {
        if (IsValidTransition(goal))
        {
            Error = null!;
            Status = goal;
            return true;
        }

        Error = InvalidTransition(goal);
        return false;
    }

    private bool IsValidTransition(VolunteerRequestStatus goal)
        => Status switch
        {
            VolunteerRequestStatus.revisionRequired or // from here
            VolunteerRequestStatus.submitted when // or here
            goal == VolunteerRequestStatus.onReview => true, // we can go here

            VolunteerRequestStatus.onReview when
            goal == VolunteerRequestStatus.revisionRequired => true,

            VolunteerRequestStatus.onReview when
            goal == VolunteerRequestStatus.rejected => true,

            VolunteerRequestStatus.onReview when
            goal == VolunteerRequestStatus.approved => true,

            _ => false
        };

    private Error InvalidTransition(VolunteerRequestStatus goal)
        => Error.Failure("invalid.state.switch", $"Unable to switch from {Status.ToString()} to {goal.ToString()}");
}

public enum VolunteerRequestStatus
{
    submitted,
    onReview,
    rejected,
    revisionRequired,
    approved
}