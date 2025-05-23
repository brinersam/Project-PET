using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectPet.SharedKernel.SharedDto;
using ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.Update;
using ProjectPet.VolunteerRequests.Contracts.Dto;
using ProjectPet.VolunteerRequests.IntegrationTests.Factories;
using ProjectPet.VolunteerRequests.IntegrationTests.VolunteerRequestsTests.Base;

namespace ProjectPet.VolunteerRequests.IntegrationTests.VolunteerRequestsTests.Commands;
public class UpdateVolunteerRequestTests : VolunteerRequestsTestBase
{
    protected UpdateHandler _sut;

    public UpdateVolunteerRequestTests(VolunteerRequestsWebFactory factory) : base(factory)
    {
        _sut = _serviceScope.ServiceProvider.GetRequiredService<UpdateHandler>();
    }

    [Fact]
    public async Task Update_VolunteerRequest_Success()
    {
        // Arrange
        var volunteerRequest = await SeedVolunteerRequestAndSetToRevisionRequired(default, default);

        var oldVolunteerData = volunteerRequest.VolunteerData;
        var newVolunteerData = _fixture.Create<VolunteerAccountDto>();

        var cmd = new UpdateCommand(volunteerRequest.Id, newVolunteerData);

        // Act
        var result = await _sut.HandleAsync(cmd, default);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var volunteerRequestAssert = await _readDbContext.VolunteerRequests.FirstOrDefaultAsync();

        // request status is on review
        volunteerRequestAssert!.Status.Should().Be(VolunteerRequestStatusDto.onReview);

        // volunteer data is new
        volunteerRequestAssert.VolunteerData.Certifications.Should().BeEquivalentTo(newVolunteerData.Certifications);
        volunteerRequestAssert.VolunteerData.Experience.Should().Be(newVolunteerData.Experience);
        volunteerRequestAssert.VolunteerData.PaymentInfos.Should().BeEquivalentTo(newVolunteerData.PaymentInfos);
    }
}
