using Domain.Entities;
using FluentValidation.TestHelper;

namespace Tests;

public class Comments
{
    private readonly CommentValidator _validator;

    public Comments()
    {
        _validator = new CommentValidator();
    }

    [Fact]
    public void Create_ShouldSetCreatedAtToCurrentUtcTime()
    {
        // Arrange
        var comment = new Comment
        {
            Content = "This is a test comment.",
            User = "TestUser"
        };

        // Act
        var result = comment.Create();

        // Assert
        Assert.Equal(comment, result); // Ensure the same instance is returned
        Assert.True((DateTime.UtcNow - comment.CreatedAt).TotalSeconds < 1, "CreatedAt should be set to the current UTC time.");
    }

    [Fact]
    public void Should_HaveError_When_TaskIdIsZero()
    {
        // Arrange
        var comment = new Comment { TaskId = 0 };

        // Act
        var result = _validator.TestValidate(comment);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.TaskId)
              .WithErrorMessage("Task ID must be greater than 0.");
    }

    [Fact]
    public void Should_HaveError_When_TaskIdIsEmpty()
    {
        // Arrange
        var comment = new Comment { TaskId = default };

        // Act
        var result = _validator.TestValidate(comment);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.TaskId)
              .WithErrorMessage("Task ID is required.");
    }

    [Fact]
    public void Should_HaveError_When_UserIsEmpty()
    {
        // Arrange
        var comment = new Comment { User = string.Empty };

        // Act
        var result = _validator.TestValidate(comment);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.User)
              .WithErrorMessage("User ID is required.");
    }

    [Fact]
    public void Should_HaveError_When_UserExceedsMaxLength()
    {
        // Arrange
        var comment = new Comment { User = new string('A', 101) };

        // Act
        var result = _validator.TestValidate(comment);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.User)
              .WithErrorMessage("User must be at most 100 characters long.");
    }

    [Fact]
    public void Should_HaveError_When_ContentIsEmpty()
    {
        // Arrange
        var comment = new Comment { Content = string.Empty };

        // Act
        var result = _validator.TestValidate(comment);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Content)
              .WithErrorMessage("Comment content is required.");
    }

    [Fact]
    public void Should_HaveError_When_ContentExceedsMaxLength()
    {
        // Arrange
        var comment = new Comment { Content = new string('A', 1001) };

        // Act
        var result = _validator.TestValidate(comment);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Content)
              .WithErrorMessage("Comment content must be at most 1000 characters long.");
    }

    [Fact]
    public void Should_NotHaveError_When_AllFieldsAreValid()
    {
        // Arrange
        var comment = new Comment
        {
            TaskId = 1,
            User = "ValidUser",
            Content = "This is a valid comment."
        };

        // Act
        var result = _validator.TestValidate(comment);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}