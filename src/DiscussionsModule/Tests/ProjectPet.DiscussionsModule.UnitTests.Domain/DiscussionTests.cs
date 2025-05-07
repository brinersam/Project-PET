using AutoFixture.Xunit2;
using FluentAssertions;
using ProjectPet.DiscussionsModule.Domain.Models;
using ProjectPet.DiscussionsModule.UnitTests.Domain.Fixtures;
using ProjectPet.DiscussionsModule.UnitTests.Domain.Fixtures.Discussions;

namespace ProjectPet.DiscussionsModule.UnitTests.Domain;

public class DiscussionTests
{
    [Theory]
    [AutoData]
    public void CreateDiscussion_ValidEntry_Success(Guid relatedEntity,
                                                    List<Guid> users)
    {
        // Act
        var sut = Discussion.Create(relatedEntity,users);

        // Assert
        sut.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineAmntOfGuids(0)]
    [InlineAmntOfGuids(1)]
    public void CreateDiscussion_UserAmnt_Invalid_Failure(Guid relatedEntity,
                                                         List<Guid> users)
    {
        // Act
        var sut = Discussion.Create(relatedEntity, users);

        // Assert
        sut.IsSuccess.Should().BeFalse();
    }

    [Theory]
    [InlineAmntOfGuids(2)]
    [InlineAmntOfGuids(3)]
    public void CreateDiscussion_UserAmnt_Valid_Success(Guid relatedEntity,
                                                        List<Guid> users)
    {
        // Act
        var sut = Discussion.Create(relatedEntity, users);

        // Assert
        sut.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [BaseDiscussion]
    public void AddComment_Participating_Success(Discussion discussion,
                                                 string text)
    {
        // Arrange
        var userId = discussion.Users.FirstOrDefault();

        // Act
        var sut = discussion.AddComment(userId, text);

        // Assert
        sut.IsSuccess.Should().BeTrue();
        discussion.Messages.Should().Contain(x => x.Text == text);
    }

    [Theory]
    [ClosedDiscussion]
    public void AddComment_Participating_ClosedDiscussion_Failure(Discussion discussion,
                                                         string text)
    {
        // Arrange
        var userId = discussion.Users.FirstOrDefault();

        // Act
        var sut = discussion.AddComment(userId, text);

        // Assert
        sut.IsSuccess.Should().BeFalse();
        discussion.Messages.Should().NotContain(x => x.Text == text);
    }

    [Theory]
    [BaseDiscussion]
    public void AddComment_NotParticipating_Failure(Discussion discussion,
                                                    string text)
    {
        // Arrange
        var userId = new Guid();

        // Act
        var sut = discussion.AddComment(userId, text);

        // Assert
        sut.IsSuccess.Should().BeFalse();
        discussion.Messages.Should().NotContain(x => x.Text == text);
    }

    [Theory]
    [CommentsDiscussion]
    public void DeleteComment_Author_Success(Discussion discussion)
    {
        // Arrange
        var userId = discussion.Users.FirstOrDefault();
        var comment = discussion.Messages.FirstOrDefault(x => x.UserId == userId);
        var commentId = comment!.Id;

        // Act
        var sut = discussion.DeleteComment(userId, commentId);

        // Assert
        sut.IsSuccess.Should().BeTrue();
        discussion.Messages.Should().NotContain(x => x.Text == comment.Text);
    }

    [Theory]
    [ClosedDiscussion]
    public void DeleteComment_Author_ClosedDiscussion_Failure(Discussion discussion)
    {
        // Arrange
        var userId = discussion.Users.FirstOrDefault();
        var comment = discussion.Messages.FirstOrDefault(x => x.UserId == userId);
        var commentId = comment!.Id;

        // Act
        var sut = discussion.DeleteComment(userId, commentId);

        // Assert
        sut.IsSuccess.Should().BeFalse();
        discussion.Messages.Should().Contain(x => x.Text == comment.Text);
    }

    [Theory]
    [CommentsDiscussion]
    public void DeleteComment_NotAuthor_Failure(Discussion discussion)
    {
        // Arrange
        var userId = discussion.Users.FirstOrDefault();
        var comment = discussion.Messages.FirstOrDefault(x => x.UserId != userId);
        var commentId = comment!.Id;
        var InitialMsgAmount = discussion.Messages.Count();

        // Act
        var sut = discussion.DeleteComment(userId, commentId);

        // Assert
        sut.IsSuccess.Should().BeFalse();
        discussion.Messages.Should().Contain(x => x.Text == comment.Text);
    }

    [Theory]
    [CommentsDiscussion]
    public void DeleteComment_CommentDoesNotExist_Success(Discussion discussion)
    {
        // Arrange
        var userId = discussion.Users.FirstOrDefault();
        var commentId = Guid.NewGuid();
        var initialMsgAmount = discussion.Messages.Count();

        // Act
        var sut = discussion.DeleteComment(userId, commentId);

        // Assert
        sut.IsSuccess.Should().BeTrue();
        discussion.Messages.Should().HaveCount(initialMsgAmount);
    }

    [Theory]
    [CommentsDiscussion]
    public void EditComment_Author_Success(Discussion discussion, string text)
    {
        // Arrange
        var userId = discussion.Users.FirstOrDefault();
        var comment = discussion.Messages.FirstOrDefault(x => x.UserId == userId);
        var commentId = comment!.Id;

        var initialText = comment.Text;
        var initialMsgAmount = discussion.Messages.Count();

        // Act
        var sut = discussion.EditComment(userId, commentId, text);

        // Assert
        sut.IsSuccess.Should().BeTrue();
        discussion.Messages.Should().HaveCount(initialMsgAmount);
        discussion.Messages.Should().Contain(x => x.Text == text);
        discussion.Messages.Should().NotContain(x => x.Text == initialText);
    }

    [Theory]
    [ClosedDiscussion]
    public void EditComment_Author_ClosedDiscussion_Failure(Discussion discussion, string text)
    {
        // Arrange
        var userId = discussion.Users.FirstOrDefault();
        var comment = discussion.Messages.FirstOrDefault(x => x.UserId == userId);
        var commentId = comment!.Id;

        var initialText = comment.Text;
        var initialMsgAmount = discussion.Messages.Count();

        // Act
        var sut = discussion.EditComment(userId, commentId, text);

        // Assert
        sut.IsSuccess.Should().BeFalse();
        discussion.Messages.Should().HaveCount(initialMsgAmount);
        discussion.Messages.Should().NotContain(x => x.Text == text);
        discussion.Messages.Should().Contain(x => x.Text == initialText);
    }

    [Theory]
    [CommentsDiscussion]
    public void EditComment_NotAuthor_Failure(Discussion discussion, string text)
    {
        // Arrange
        var userId = discussion.Users.FirstOrDefault();
        var comment = discussion.Messages.FirstOrDefault(x => x.UserId != userId);
        var commentId = comment!.Id;

        var initialText = comment.Text;
        var initialMsgAmount = discussion.Messages.Count();

        // Act
        var sut = discussion.EditComment(userId, commentId, text);

        // Assert
        sut.IsSuccess.Should().BeFalse();
        discussion.Messages.Should().HaveCount(initialMsgAmount);
        discussion.Messages.Should().Contain(x => x.Text == initialText);
        discussion.Messages.Should().NotContain(x => x.Text == text);
    }

    [Theory]
    [BaseDiscussion]
    public void EndDiscussion_Success(Discussion discussion)
    {
        // Act
        var sut = discussion.EndDiscussion();

        // Assert
        sut.IsSuccess.Should().BeTrue();
        discussion.IsClosed.Should().BeTrue();
    }
}
