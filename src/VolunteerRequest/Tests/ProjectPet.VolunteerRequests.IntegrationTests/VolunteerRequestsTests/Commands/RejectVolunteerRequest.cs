using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectPet.Framework.Authorization;
using ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.Reject;
using ProjectPet.VolunteerRequests.Contracts.Dto;
using ProjectPet.VolunteerRequests.IntegrationTests.Factories;
using ProjectPet.VolunteerRequests.IntegrationTests.VolunteerRequestsTests.Base;

namespace ProjectPet.VolunteerRequests.IntegrationTests.VolunteerRequestsTests.Commands;
public class RejectVolunteerRequest : VolunteerRequestsTestBase
{
    protected RejectHandler _sut;

    public RejectVolunteerRequest(VolunteerRequestsWebFactory factory) : base(factory)
    {
        _sut = _serviceScope.ServiceProvider.GetRequiredService<RejectHandler>();
    }

    [Fact]
    public async Task Reject_VolunteerRequest__Success()
    {
        // Arrange
        var user = await SeedUserAsync();
        var volunteerRequest = await SeedVolunteerRequestAndSetToReview(default, user.Id);

        var rejectMsg = "rejected";

        var cmd = new RejectCommand(rejectMsg, volunteerRequest.Id, PermissionCodes.VolunteerRequestCreate);

        // Act
        var result = await _sut.HandleAsync(cmd, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var volunteerRequestAssert = await _readDbContextVR.VolunteerRequests.FirstOrDefaultAsync();

        volunteerRequestAssert!.Status.Should().Be(VolunteerRequestStatusDto.rejected);
        volunteerRequestAssert.RejectionComment.Should().BeEquivalentTo(rejectMsg);

        var permissionMod = _authDbContext.PermissionModifiers.FirstOrDefault();
        permissionMod!.UserId.Should().Be(user.Id);
        permissionMod!.Code.Should().Be(PermissionCodes.VolunteerRequestCreate);
        permissionMod!.IsAllowed.Should().BeFalse();
    }
}
