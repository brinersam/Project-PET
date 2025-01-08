using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.UploadPetPhoto;
using ProjectPet.VolunteerModule.IntegrationTests.Factories;
using ProjectPet.VolunteerModule.IntegrationTests.VolunteerTests.Base;

namespace ProjectPet.VolunteerModule.IntegrationTests.VolunteerTests;
public class UploadPetPhotoTests : VolunteerTestBase
{
    private readonly UploadPetPhotoHandler _sut;

    public UploadPetPhotoTests(VolunteerWebFactory factory) : base(factory)
    {
        _sut = _serviceScope.ServiceProvider.GetRequiredService<UploadPetPhotoHandler>();
    }

    [Fact]
    public async Task UploadPetPhoto_Success()
    {
        // Arrange
        var photoCount = _factory.IFileProviderMock_UploadFilesAsync_Success();

        var volunteerWithPet = await SeedVolunteerWithPetAsync();

        var cmd = _fixture.Build<UploadPetPhotoCommand>()
            .With(cmd => cmd.PetId, volunteerWithPet.pet.Id)
            .With(cmd => cmd.VolunteerId, volunteerWithPet.volunteer.Id)
            .Create();

        // Act
        var result = await _sut.HandleAsync(cmd);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        volunteerWithPet.pet.Photos.Data.Should().HaveCount(photoCount);
    }

    [Fact]
    public async Task UploadPetPhoto_FileUploadFailed_PetHasNoPhotoRefs()
    {
        // Arrange
        _factory.IFileProviderMock_UploadFilesAsync_Failure();

        var volunteerWithPet = await SeedVolunteerWithPetAsync();

        var cmd = _fixture.Build<UploadPetPhotoCommand>()
            .With(cmd => cmd.PetId, volunteerWithPet.pet.Id)
            .With(cmd => cmd.VolunteerId, volunteerWithPet.volunteer.Id)
            .Create();

        // Act
        var result = await _sut.HandleAsync(cmd);

        // Assert
        result.IsSuccess.Should().BeFalse();
        volunteerWithPet.pet.Photos.Data.Should().HaveCount(0);
    }
}
