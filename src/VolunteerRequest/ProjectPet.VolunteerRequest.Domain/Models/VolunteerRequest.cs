using CSharpFunctionalExtensions;
using ProjectPet.Core.Validator;
using ProjectPet.SharedKernel;
using ProjectPet.SharedKernel.Entities.AbstractBase;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SharedKernel.SharedDto;

namespace ProjectPet.VolunteerRequest.Domain.Models;
public class VolunteerRequest : EntityBase
{
    public Guid AdminId { get; private set; }
    public Guid UserId { get; private set; }
    public Guid DiscussionId { get; private set; }
    public VolunteerAccountDto VolunteerData { get; private set; }
    public VolunteerRequestStatus RequestStatus { get; private set; }
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
        VolunteerAccountDto volunteerData,
        VolunteerRequestStatus requestStatus,
        DateTime createdAt,
        string rejectionComment) : base(id)
    {
        AdminId = adminId;
        UserId = userId;
        DiscussionId = discussionId;
        VolunteerData = volunteerData;
        RequestStatus = requestStatus;
        CreatedAt = createdAt;
        RejectionComment = rejectionComment;
    }

    public static Result<VolunteerRequest,Error> Create(
        Guid userId,
        Guid discussionId,
        VolunteerAccountDto volunteerData)
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

    public UnitResult<Error> BeginReview(Guid adminId)
    {
        if (RequestStatus != VolunteerRequestStatus.submitted &&
            RequestStatus != VolunteerRequestStatus.revisionRequired)
        {
            return Errors.General.ValueIsInvalid(RequestStatus, nameof(VolunteerRequestStatus));
        }

        AdminId = adminId;
        RequestStatus = VolunteerRequestStatus.onReview;
        return Result.Success<Error>();
    }

    public UnitResult<Error> RequestRevision(string rejectionComment)
    {
        if (RequestStatus != VolunteerRequestStatus.onReview)
            return Errors.General.ValueIsInvalid(RequestStatus, nameof(VolunteerRequestStatus));

        var validatorStr = Validator.ValidatorString(Constants.STRING_LEN_MEDIUM);
        var result = validatorStr.Check(rejectionComment, nameof(rejectionComment));
        if (result.IsFailure)
            return result.Error;

        RejectionComment = rejectionComment;
        RequestStatus = VolunteerRequestStatus.revisionRequired;
        return Result.Success<Error>();
    }

    public UnitResult<Error> RejectRequest(string rejectionComment = "")
    {
        if (RequestStatus != VolunteerRequestStatus.onReview)
            return Errors.General.ValueIsInvalid(RequestStatus, nameof(VolunteerRequestStatus));

        RejectionComment = rejectionComment;
        RequestStatus = VolunteerRequestStatus.rejected;
        return Result.Success<Error>();
    }

    public UnitResult<Error> ApproveRequest()
    {
        if (RequestStatus != VolunteerRequestStatus.onReview)
            return Errors.General.ValueIsInvalid(RequestStatus, nameof(VolunteerRequestStatus));

        RequestStatus = VolunteerRequestStatus.approved;
        return Result.Success<Error>();
    }
}

public enum VolunteerRequestStatus
{
    submitted,
    onReview,
    rejected,
    revisionRequired,
    approved
}