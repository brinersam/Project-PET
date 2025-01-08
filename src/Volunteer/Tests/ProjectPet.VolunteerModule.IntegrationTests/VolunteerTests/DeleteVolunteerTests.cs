using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.DeleteVolunteer;
using ProjectPet.VolunteerModule.IntegrationTests.Factories;
using ProjectPet.VolunteerModule.IntegrationTests.VolunteerTests.Base;

namespace ProjectPet.VolunteerModule.IntegrationTests.VolunteerTests;
public class DeleteVolunteerTests : VolunteerTestBase
{
    private DeleteVolunteerHandler _sut;

    public DeleteVolunteerTests(VolunteerWebFactory factory) : base(factory)
    {
        _sut = _serviceScope.ServiceProvider.GetRequiredService<DeleteVolunteerHandler>();
    }

    [Fact]
    public async Task Delete_NonExisting_Failure()
    {
        // Arrange
        var cmd = new DeleteVolunteerCommand(Guid.NewGuid(), false);

        // Act
        var result = await _sut.HandleAsync(cmd);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Delete_WithPet_Success()
    {
        // Arrange
        var volunteerAndPet = await SeedVolunteerWithPetAsync();
        var cmd = new DeleteVolunteerCommand(volunteerAndPet.volunteer.Id, false);

        // Act
        var result = await _sut.HandleAsync(cmd);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _readDbContext.Volunteers.Should().HaveCount(0);
        _readDbContext.Pets.Should().HaveCount(0);
    }

    [Fact]
    public async Task SoftDelete_WithPet_HiddenInReadContext_ShownInWriteContext()
    {
        // Arrange
        var volunteerAndPet = await SeedVolunteerWithPetAsync();
        var cmd = new DeleteVolunteerCommand(volunteerAndPet.volunteer.Id, true);

        // Act
        var result = await _sut.HandleAsync(cmd);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _readDbContext.Volunteers.Should().HaveCount(0);
        _readDbContext.Pets.Should().HaveCount(0);

        _writeDbContext.Volunteers.Should().HaveCount(1);
        _writeDbContext.Volunteers.First().OwnedPets.Should().HaveCount(1);
    }

    [Fact]
    public async Task SoftDeleteThenRestore_WithPet_AccessibleFromReadContext()
    {
        // Arrange
        var volunteerAndPet = await SeedVolunteerWithPetAsync();
        var cmd = new DeleteVolunteerCommand(volunteerAndPet.volunteer.Id, true);
        var result = await _sut.HandleAsync(cmd);

        // Act
        var volunteer = _writeDbContext.Volunteers.First();
        volunteer.SoftRestore();
        _writeDbContext.SaveChanges();

        // Assert
        result.IsSuccess.Should().BeTrue();
        _readDbContext.Volunteers.Should().HaveCount(1);
        _readDbContext.Pets.Should().HaveCount(1);
    }

    [Fact]
    public async Task SoftDelete_Twice_IdempotentDate()
    {
        // Arrange
        var volunteerAndPet = await SeedVolunteerWithPetAsync();
        var cmd = new DeleteVolunteerCommand(volunteerAndPet.volunteer.Id, true);

        // Act
        var result1 = await _sut.HandleAsync(cmd);
        _writeDbContext.SaveChanges();
        var delTime1 = _writeDbContext.Volunteers.First().DeletionDate;

        await Task.Delay(2000);

        var result2 = await _sut.HandleAsync(cmd);
        _writeDbContext.SaveChanges();
        var delTime2 = _writeDbContext.Volunteers.First().DeletionDate;

        // Assert
        result1.IsSuccess.Should().BeTrue();
        result2.IsSuccess.Should().BeTrue();
        delTime1.Should().BeCloseTo(delTime2, TimeSpan.FromSeconds(1));
    }
}
