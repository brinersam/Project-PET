using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using ProjectPet.DiscussionsModule.Domain.Models;
using ProjectPet.DiscussionsModule.UnitTests.Domain.Fixtures;
using ProjectPet.DiscussionsModule.UnitTests.Domain.Fixtures.Messages.Customizations;
using ProjectPet.SharedKernel;

namespace ProjectPet.DiscussionsModule.UnitTests.Domain;
public class MessageTests
{
    [Theory]
    [AutoData]
    public void CreateMessage_ValidData_Success(Guid userId,
                                                string text)
    {
        // Act
        var sut = Message.Create(userId, text);

        // Assert
        sut.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [AutoData]
    public void CreateMessage_ValidData_Message_Is_NotEdited(Guid userId,
                                                             string text)
    {
        // Act
        var sut = Message.Create(userId, text);

        // Assert
        sut.IsSuccess.Should().BeTrue();
        sut.Value.IsEdited.Should().BeFalse();
    }

    [Theory]
    [InlineAutoData("")]
    [InlineAutoData("    ")]
    [InlineStringWithLengthData(Constants.STRING_LEN_MEDIUM + 1)]
    public void CreateMessage_InvalidText_Failure(string text,
                                                  Guid userId)
    {
        // Act
        var sut = Message.Create(userId, text);

        // Assert
        sut.IsSuccess.Should().BeFalse();
    }

    [Theory]
    [AutoData]
    public void UpdateMessage_ValidText_Success(string text)
    {
        // Arrange
        var message = CreateMessage();

        // Act
        var sut = message.Update(text);

        // Assert
        sut.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [AutoData]
    public void UpdateMessage_ValidText_Message_Is_Edited(string text)
    {
        // Arrange
        var message = CreateMessage();

        // Act
        var sut = message.Update(text);

        // Assert
        sut.IsSuccess.Should().BeTrue();
        sut.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineAutoData("")]
    [InlineAutoData("    ")]
    [InlineStringWithLengthData(Constants.STRING_LEN_MEDIUM + 1)]
    public void UpdateMessage_InvalidText_Failure(string text)
    {
        // Arrange
        var fixture = new Fixture();
        var message = fixture
            .Customize(new BaseMessageCustomization())
            .Create<Message>();

        // Act
        var sut = message.Update(text);

        // Assert
        sut.IsSuccess.Should().BeFalse();
    }

    private Message CreateMessage()
    {
        var fixture = new Fixture();
        return fixture
            .Customize(new BaseMessageCustomization())
            .Create<Message>();
    }
}
