using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.RequestRevision;
using ProjectPet.VolunteerRequests.Contracts.Dto;
using ProjectPet.VolunteerRequests.IntegrationTests.Factories;
using ProjectPet.VolunteerRequests.IntegrationTests.VolunteerRequestsTests.Base;

namespace ProjectPet.VolunteerRequests.IntegrationTests.VolunteerRequestsTests.Commands;
public class RequestRevisionVolunteerRequestTests : VolunteerRequestsTestBase
{
    private RequestRevisionHandler _sut;

    public RequestRevisionVolunteerRequestTests(VolunteerRequestsWebFactory factory) : base(factory)
    {
        _sut = _serviceScope.ServiceProvider.GetRequiredService<RequestRevisionHandler>();
    }

    [Fact]
    public async Task VolunteerRequest_RequestRevision_Success()
    {
        // Arrange
        var volunteerRequest = await SeedVolunteerRequestAsync();
        var adminId = _fixture.Create<Guid>();
        volunteerRequest.BeginReview(adminId);

        var revisionMsg = "rejected";
        var cmd = new RequestRevisionCommand(revisionMsg, volunteerRequest.Id);

        // Act
        var result = await _sut.HandleAsync(cmd, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var volunteerRequestAssert = await _readDbContextVR.VolunteerRequests.FirstOrDefaultAsync();
        volunteerRequestAssert!.Status.Should().Be(VolunteerRequestStatusDto.revisionRequired);
        volunteerRequestAssert.RejectionComment.Should().BeEquivalentTo(revisionMsg);
    }
}
