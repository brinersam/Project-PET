using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectPet.SharedKernel.SharedDto;
using ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.Create;
using ProjectPet.VolunteerRequests.IntegrationTests.Factories;
using ProjectPet.VolunteerRequests.IntegrationTests.VolunteerRequestsTests.Base;

namespace ProjectPet.VolunteerRequests.IntegrationTests.VolunteerRequestsTests.Commands;
public class CreateVolunteerRequestTests : VolunteerRequestsTestBase
{
    private CreateHandler _sut;

    public CreateVolunteerRequestTests(VolunteerRequestsWebFactory factory) : base(factory)
    {
        _sut = _serviceScope.ServiceProvider.GetRequiredService<CreateHandler>();
    }

    [Fact]
    public async Task Create_VolunteerRequest_Success()
    {
        // Arrange
        var accountDto = _fixture.Create<VolunteerAccountDto>();
        var userId = _fixture.Create<Guid>();

        var cmd = new CreateCommand(accountDto, userId);

        // Act
        var result = await _sut.HandleAsync(cmd, default);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var volunteerRequestAssert = await _readDbContextVR.VolunteerRequests.FirstOrDefaultAsync();

        volunteerRequestAssert.Should().NotBeNull();
        volunteerRequestAssert!.UserId.Should().Be(userId);
    }
}
