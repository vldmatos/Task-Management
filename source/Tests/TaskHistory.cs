using Domain.Entities;
using FluentValidation.TestHelper;

namespace Tests;

public class TasksHistory
{
    private readonly TasksHistoryValidator _validator;

    public TasksHistory()
    {
        _validator = new TasksHistoryValidator();
    }

    [Fact]
    public void Should_HaveError_When_TaskIdIsZero()
    {
        // Arrange
        var taskHistory = new TaskHistory { TaskId = 0 };

        // Act
        var result = _validator.TestValidate(taskHistory);

        // Assert
        result.ShouldHaveValidationErrorFor(th => th.TaskId)
              .WithErrorMessage("Task ID must be greater than 0.");
    }

    [Fact]
    public void Should_HaveError_When_TaskIdIsEmpty()
    {
        // Arrange
        var taskHistory = new TaskHistory { TaskId = default };

        // Act
        var result = _validator.TestValidate(taskHistory);

        // Assert
        result.ShouldHaveValidationErrorFor(th => th.TaskId)
              .WithErrorMessage("Task ID is required.");
    }

    [Fact]
    public void Should_HaveError_When_UserIsEmpty()
    {
        // Arrange
        var taskHistory = new TaskHistory { User = string.Empty };

        // Act
        var result = _validator.TestValidate(taskHistory);

        // Assert
        result.ShouldHaveValidationErrorFor(th => th.User)
              .WithErrorMessage("User ID is required.");
    }

    [Fact]
    public void Should_HaveError_When_UserExceedsMaxLength()
    {
        // Arrange
        var taskHistory = new TaskHistory { User = new string('A', 101) };

        // Act
        var result = _validator.TestValidate(taskHistory);

        // Assert
        result.ShouldHaveValidationErrorFor(th => th.User)
              .WithErrorMessage("User must be at most 100 characters long.");
    }

    [Fact]
    public void Should_HaveError_When_ChangeIsEmpty()
    {
        // Arrange
        var taskHistory = new TaskHistory { Change = string.Empty };

        // Act
        var result = _validator.TestValidate(taskHistory);

        // Assert
        result.ShouldHaveValidationErrorFor(th => th.Change)
              .WithErrorMessage("Field change is required.");
    }

    [Fact]
    public void Should_HaveError_When_ChangeExceedsMaxLength()
    {
        // Arrange
        var taskHistory = new TaskHistory { Change = new string('A', 1001) };

        // Act
        var result = _validator.TestValidate(taskHistory);

        // Assert
        result.ShouldHaveValidationErrorFor(th => th.Change)
              .WithErrorMessage("Field changed must be at most 1000 characters long.");
    }

    [Fact]
    public void Should_NotHaveError_When_AllFieldsAreValid()
    {
        // Arrange
        var taskHistory = new TaskHistory
        {
            TaskId = 1,
            User = "ValidUser",
            Change = "Field was updated.",
            ChangedAt = DateTime.UtcNow
        };

        // Act
        var result = _validator.TestValidate(taskHistory);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}

