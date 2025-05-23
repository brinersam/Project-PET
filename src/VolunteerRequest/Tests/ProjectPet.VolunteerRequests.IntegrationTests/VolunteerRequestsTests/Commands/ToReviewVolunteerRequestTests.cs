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
    public async Task VolunteerRequest_ToReview__Success()
    {
        // Arrange
        var volunteerRequest = await SeedVolunteerRequestAsync();
        var adminId = _fixture.Create<Guid>();

        var cmd = new ReviewCommand(volunteerRequest.Id, adminId);

        // Act
        var result = await _sut.HandleAsync(cmd, default);

        // Assert
        var discussion = await _discussionsReadDbContext.Discussions.FirstOrDefaultAsync();

        result.IsSuccess.Should().BeTrue();
        volunteerRequest.Status.Should().Be(VolunteerRequestStatus.onReview);
        discussion.Should().NotBeNull();
        discussion.UserIds.Should().Contain(adminId);
        discussion.UserIds.Should().Contain(volunteerRequest.UserId);
        discussion.UserIds.Should().HaveCount(2);
    }

    [Fact]
    public async Task VolunteerRequest_ToReview_DiscussionContractFailure_RequestNotCreated()
    {
        // Arrange
        _factory.IDiscussionModuleContractMock_CreateDiscussionAsync_Failure();

        var volunteerRequest = await SeedVolunteerRequestAsync();
        var initialStatus = volunteerRequest.Status;
        var adminId = _fixture.Create<Guid>();

        var cmd = new ReviewCommand(volunteerRequest.Id, adminId);

        // Act
        var result = await _sut.HandleAsync(cmd, default);

        // Assert
        var discussion = await _discussionsReadDbContext.Discussions.FirstOrDefaultAsync();

        result.IsSuccess.Should().BeFalse();

        volunteerRequest.Status.Should().Be(initialStatus);
        discussion.Should().BeNull();
    }

    //[Fact]
    //public async Task VolunteerRequest_ToReview_RequestCreationFailure_DiscussionIsNotCreated()
    // if domain method would fail, then this will cover that case (error in not doing both db changes though transaction)

}