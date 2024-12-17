using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.CreateVolunteer;
using ProjectPet.VolunteerModule.IntegrationTests.Factories;

namespace ProjectPet.VolunteerModule.IntegrationTests.VolunteerTests;

public class AddVolunteerTests : VolunteerTestBase
{
    private readonly CreateVolunteerHandler _sut;

    public AddVolunteerTests(VolunteerWebFactory factory) : base(factory)
    {
        _sut = _serviceScope.ServiceProvider.GetRequiredService<CreateVolunteerHandler>();
    }

    [Fact]
    public async Task Add_Volunteer_ToDB()
    {
        // Arrange
        var cmd = _fixture.Create<CreateVolunteerCommand>();

        // Act
        var result = await _sut.HandleAsync(cmd);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        _readDbContext.Volunteers.Should().HaveCount(1);
    }
}