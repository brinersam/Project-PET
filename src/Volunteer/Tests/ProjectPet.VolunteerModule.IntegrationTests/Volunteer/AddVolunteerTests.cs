using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.CreateVolunteer;
using ProjectPet.VolunteerModule.Application.Interfaces;
using ProjectPet.VolunteerModule.Contracts.Dto;
using ProjectPet.VolunteerModule.IntegrationTests.Factories;

namespace ProjectPet.VolunteerModule.IntegrationTests.Volunteer;

public class AddVolunteerTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    private readonly Fixture _fixture;
    private readonly IntegrationTestsWebFactory _factory;
    private readonly IServiceScope _serviceScope;
    private readonly IReadDbContext _readDbContext;


    public AddVolunteerTests(IntegrationTestsWebFactory factory)
    {
        _fixture = new Fixture();
        _factory = factory;
        _serviceScope = _factory.Services.CreateScope();
        _readDbContext = _serviceScope.ServiceProvider.GetRequiredService<IReadDbContext>();
    }

    public Task DisposeAsync()
    {
        _serviceScope.Dispose();
        return Task.CompletedTask;
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public async Task Add_Volunteer_ToDB()
    {
        // Arrange
        var dto = _fixture.Build<CreateVolunteerDto>()
            .With(x => x.Phonenumber, new PhonenumberDto("123-456-78-99", "+4"))
            .Create();

        var cmd = _fixture.Build<CreateVolunteerCommand>()
            .With(x => x.VolunteerDto, dto)
            .Create();

        var sut = _serviceScope.ServiceProvider.GetRequiredService<CreateVolunteerHandler>();

        // Act
        var result = await sut.HandleAsync(cmd);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        _readDbContext.Volunteers.Should().HaveCount(1);
    }

}