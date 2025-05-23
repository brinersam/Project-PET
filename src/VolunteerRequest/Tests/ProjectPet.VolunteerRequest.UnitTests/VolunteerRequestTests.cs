using AutoFixture;
using CSharpFunctionalExtensions;
using FluentAssertions;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SharedKernel.SharedDto;
using ProjectPet.VolunteerRequests.Domain.Models;

namespace ProjectPet.VolunteerRequests.UnitTests;

public class VolunteerRequestTests
{
    Fixture _fixture;

    public VolunteerRequestTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void NewRequest_StateEqualsTo_Submitted()
    {
        // arrange
        var accountDto = _fixture.Create<VolunteerAccountDto>();

        // act
        var sut = VolunteerRequest.Create(
            Guid.Empty,
            Guid.Empty,
            accountDto);

        // assert
        sut.IsSuccess.Should().BeTrue();
        sut.Value.Status.Should().Be(VolunteerRequestStatus.submitted);
    }

    [Theory]
    [InlineData(VolunteerRequestStatus.submitted, VolunteerRequestStatus.onReview)]
    [InlineData(VolunteerRequestStatus.onReview, VolunteerRequestStatus.revisionRequired)]
    [InlineData(VolunteerRequestStatus.revisionRequired, VolunteerRequestStatus.onReview)]
    [InlineData(VolunteerRequestStatus.onReview, VolunteerRequestStatus.rejected)]
    [InlineData(VolunteerRequestStatus.onReview, VolunteerRequestStatus.approved)]
    public void Transitions_Correct_Success(
        VolunteerRequestStatus startingState,
        VolunteerRequestStatus goalState)
    {
        // arrange
        var vr = CreateVolunteerRequest();
        vr = InitState(vr, startingState);

        // act
        UnitResult<Error> result = AttemptTransition(vr, goalState);

        // assert
        result.IsSuccess.Should().BeTrue();
    }

    private VolunteerRequest InitState(
        VolunteerRequest volunteerRequest,
        VolunteerRequestStatus desiredState)
    {
        switch (desiredState)
        {
            case VolunteerRequestStatus.onReview:
                {
                    volunteerRequest.BeginReview(new Guid());
                    break;
                }
            case VolunteerRequestStatus.rejected:
                {
                    volunteerRequest.BeginReview(new Guid());
                    volunteerRequest.RejectRequest("Rejected");
                    break;
                }
            case VolunteerRequestStatus.revisionRequired:
                {
                    volunteerRequest.BeginReview(new Guid());
                    volunteerRequest.RequestRevision("RequestedRevision");
                    break;
                }
            case VolunteerRequestStatus.approved:
                {
                    volunteerRequest.BeginReview(new Guid());
                    volunteerRequest.ApproveRequest();
                    break;
                }
        }
        return volunteerRequest;
    }

    private UnitResult<Error> AttemptTransition(
        VolunteerRequest vr,
        VolunteerRequestStatus transitionalState)
    {
        return transitionalState switch
        {
            VolunteerRequestStatus.onReview => vr.BeginReview(Guid.Empty),
            VolunteerRequestStatus.rejected => vr.RejectRequest("  сожалению, сейчас мы не готовы пригласить ¬ас на следующий этап"),
            VolunteerRequestStatus.revisionRequired => vr.RequestRevision("переделывай"),
            VolunteerRequestStatus.approved => vr.ApproveRequest(),
            _ => UnitResult.Failure(Error.Failure("illegal", "illegal transition"))
        };
    }

    #region Data Generation
    private VolunteerRequest CreateVolunteerRequest(
        Guid userId = default,
        Guid discussionId = default,
        VolunteerAccountDto dto = null!)
    {
        var createResult = VolunteerRequest.Create(
            userId == default ?         Guid.NewGuid() : userId,
            discussionId == default ?   Guid.NewGuid() : discussionId,
            dto ??                      _fixture.Create<VolunteerAccountDto>());

        return createResult.Value;
    }
    #endregion
}