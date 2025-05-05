using AutoFixture;
using CSharpFunctionalExtensions;
using FluentAssertions;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SharedKernel.SharedDto;
using ProjectPet.VolunteerRequests.Domain.Models;
using System.Reflection;

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
    public void Transitions_Correct_Success(VolunteerRequestStatus stateFrom, VolunteerRequestStatus stateTo)
    {
        // arrange
        // act
        UnitResult<Error> result = CheckTransition(stateFrom, stateTo);

        // assert
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(VolunteerRequestStatus.approved, VolunteerRequestStatus.submitted)]
    [InlineData(VolunteerRequestStatus.rejected, VolunteerRequestStatus.submitted)]
    [InlineData(VolunteerRequestStatus.rejected, VolunteerRequestStatus.onReview)]
    [InlineData(VolunteerRequestStatus.rejected, VolunteerRequestStatus.revisionRequired)]
    [InlineData(VolunteerRequestStatus.approved, VolunteerRequestStatus.onReview)]
    [InlineData(VolunteerRequestStatus.onReview, VolunteerRequestStatus.submitted)]
    [InlineData(VolunteerRequestStatus.submitted, VolunteerRequestStatus.approved)]
    [InlineData(VolunteerRequestStatus.submitted, VolunteerRequestStatus.rejected)]
    [InlineData(VolunteerRequestStatus.revisionRequired, VolunteerRequestStatus.submitted)]
    public void Transitions_Incorrect_Failure(VolunteerRequestStatus stateFrom, VolunteerRequestStatus stateTo)
    {
        // arrange
        // act
        UnitResult<Error> result = CheckTransition(stateFrom, stateTo);

        // assert
        result.IsSuccess.Should().BeFalse();
    }

    private UnitResult<Error> CheckTransition(VolunteerRequestStatus stateFrom, VolunteerRequestStatus stateTo)
    {
        // arrange
        var vr = CreateVolunteerRequest();
        ForceStateTo(vr, stateFrom);

        // act
        UnitResult<Error> result = stateTo switch
        {
            VolunteerRequestStatus.onReview => vr.BeginReview(Guid.Empty),
            VolunteerRequestStatus.rejected => vr.RejectRequest("  сожалению, сейчас мы не готовы пригласить ¬ас на следующий этап"),
            VolunteerRequestStatus.revisionRequired => vr.RequestRevision("переделывай"),
            VolunteerRequestStatus.approved => vr.ApproveRequest(),
            _ => UnitResult.Failure(Error.Failure("illegal", "illegal transition"))
        };
        return result;
    }

    private void ForceStateTo(VolunteerRequest volunteerRequest, VolunteerRequestStatus goalState)
    {
        var property = typeof(VolunteerRequest).GetProperty(nameof(volunteerRequest.Status), BindingFlags.Instance | BindingFlags.Public);
        property!.SetValue(volunteerRequest, goalState);
    }

    #region Data Generation
    private VolunteerRequest CreateVolunteerRequest(
        Guid userId = default,
        Guid discussionId = default,
        VolunteerAccountDto dto = null!)
    {
        var createResult = VolunteerRequest.Create(
            userId == default ?         Guid.Empty : userId,
            discussionId == default ?   Guid.Empty : discussionId,
            dto ??                      _fixture.Create<VolunteerAccountDto>());

        return createResult.Value;
    }
    #endregion
}