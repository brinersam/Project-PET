using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectPet.DiscussionsModule.Application.Interfaces;
using ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.Review;
using ProjectPet.VolunteerRequests.Domain.Models;
using ProjectPet.VolunteerRequests.IntegrationTests.Factories;
using ProjectPet.VolunteerRequests.IntegrationTests.VolunteerRequestsTests.Base;

namespace ProjectPet.VolunteerRequests.IntegrationTests.VolunteerRequestsTests.Commands;
public class ToReviewVolunteerRequestTests : VolunteerRequestsTestBase
{
    protected ReviewHandler _sut;
    protected IReadDbContext _discussionsReadDbContext;

    public ToReviewVolunteerRequestTests(VolunteerRequestsWebFactory factory) : base(factory)
    {
        _sut = _serviceScope.ServiceProvider.GetRequiredService<ReviewHandler>();
        _discussionsReadDbContext = _serviceScope.ServiceProvider.GetRequiredService<IReadDbContext>();
    }

    [Fact]
    public async Task VolunteerRequest_ToReview_Success()
    {
        // Arrange
        var volunteerRequest = await SeedVolunteerRequestAsync();
        var adminId = _fixture.Create<Guid>();

        var cmd = new ReviewCommand(volunteerRequest.Id, adminId);

        // Act
        var result = await _sut.HandleAsync(cmd, default);

        // Assert
        var readVolunteerRequest = await _readDbContext.VolunteerRequests.FirstOrDefaultAsync(x => x.Id == volunteerRequest.Id);
        var discussion = await _discussionsReadDbContext.Discussions.FirstOrDefaultAsync();

        result.IsSuccess.Should().BeTrue();
        readVolunteerRequest.Status.Should().Be((Contracts.Dto.VolunteerRequestStatusDto)VolunteerRequestStatus.onReview);
        discussion.Should().NotBeNull();
        discussion.UserIds.Should().Contain(adminId);
        discussion.UserIds.Should().Contain(volunteerRequest.UserId);
        discussion.UserIds.Should().HaveCount(2);
    }

    //[Fact] obsolete as of replacing contract with domain events
    //public async Task VolunteerRequest_ToReview_DiscussionContractFailure_DiscussionAndRequestNotCreated()
    //{
    //    // Arrange
    //    _factory.IDiscussionModuleContractMock_CreateDiscussionAsync_Failure();

    //    var volunteerRequest = await SeedVolunteerRequestAsync();
    //    var initialStatus = volunteerRequest.Status;
    //    var adminId = _fixture.Create<Guid>();

    //    var cmd = new ReviewCommand(volunteerRequest.Id, adminId);

    //    // Act
    //    var result = await _sut.HandleAsync(cmd, default);

    //    // Assert
    //    var discussion = await _discussionsReadDbContext.Discussions.FirstOrDefaultAsync();

    //    result.IsSuccess.Should().BeFalse();

    //    volunteerRequest.Status.Should().Be(initialStatus);
    //    discussion.Should().BeNull();
    //}

    [Fact]
    public async Task VolunteerRequest_ToReview_CreateDiscussionEventHandlerFailure_DiscussionNotCreatedRequestNotChanged()
    {
        // Arrange
        _factory.CreateDiscussionEventHandlerMock_Handle_ThrowException();

        var volunteerRequest = await SeedVolunteerRequestAsync();
        var initialStatus = volunteerRequest.Status;
        var adminId = _fixture.Create<Guid>();

        var cmd = new ReviewCommand(volunteerRequest.Id, adminId);

        // Act
        var result = await _sut.HandleAsync(cmd, default);

        // Assert
        var readVolunteerRequesst = await _readDbContext.VolunteerRequests.FirstOrDefaultAsync(x => x.Id == volunteerRequest.Id);
        var discussion = await _discussionsReadDbContext.Discussions.FirstOrDefaultAsync();

        result.IsSuccess.Should().BeFalse();

        readVolunteerRequesst.Status.Should().Be((Contracts.Dto.VolunteerRequestStatusDto)initialStatus);
        discussion.Should().BeNull();
    }

    [Fact]
    public async Task VolunteerRequest_ToReview_RequestCreationFailure_DiscussionNotCreatedRequestNotChanged()
    {
        // Arrange
        _factory.IVolunteerRequestRepository_Save_Fail();

        var volunteerRequest = await SeedVolunteerRequestAsync();
        var initialStatus = volunteerRequest.Status;
        var adminId = _fixture.Create<Guid>();

        var cmd = new ReviewCommand(volunteerRequest.Id, adminId);

        // Act
        var result = await _sut.HandleAsync(cmd, default);

        // Assert
        var readVolunteerRequesst = await _readDbContext.VolunteerRequests.FirstOrDefaultAsync(x => x.Id == volunteerRequest.Id);
        var discussion = await _discussionsReadDbContext.Discussions.FirstOrDefaultAsync();

        result.IsSuccess.Should().BeFalse();

        readVolunteerRequesst.Status.Should().Be((Contracts.Dto.VolunteerRequestStatusDto)initialStatus);
        discussion.Should().BeNull();
    }

}