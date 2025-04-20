using Domain;
using Domain.Entities;
using FluentValidation.TestHelper;

namespace Tests;

public class Tasks
{
    private readonly TaskValidator _validator;
    public Tasks()
    {
        _validator = new TaskValidator();
    }

    [Fact]
    public void Change_ShouldRecordChangesInHistory()
    {
        // Arrange
        var task = new Domain.Entities.Task
        {
            Id = 1,
            Title = "Initial Title",
            Description = "Initial Description",
            DueDate = DateTime.UtcNow.AddDays(5),
            Status = Domain.TaskStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            User = "User1",
            ProjectId = 1
        };

        var updatedTask = new Domain.Entities.Task
        {
            Id = 1,
            Title = "Updated Title",
            Description = "Updated Description",
            DueDate = DateTime.UtcNow.AddDays(10),
            Status = Domain.TaskStatus.InProgress,
            CreatedAt = task.CreatedAt,
            User = "User1",
            ProjectId = 1
        };

        // Act
        var taskHistory = task.GetChanges(updatedTask);

        // Assert
        Assert.NotEmpty(taskHistory);
        Assert.Equal(4, taskHistory.Count);
    }

    [Fact]
    public void Should_HaveError_When_TitleIsEmpty()
    {
        // Arrange
        var task = new Domain.Entities.Task { Title = string.Empty };

        // Act
        var result = _validator.TestValidate(task);

        // Assert
        result.ShouldHaveValidationErrorFor(t => t.Title)
              .WithErrorMessage("Task title is required.");
    }

    [Fact]
    public void Should_HaveError_When_TitleExceedsMaxLength()
    {
        // Arrange
        var task = new Domain.Entities.Task { Title = new string('A', 101) };

        // Act
        var result = _validator.TestValidate(task);

        // Assert
        result.ShouldHaveValidationErrorFor(t => t.Title)
              .WithErrorMessage("Task title must be at most 100 characters long.");
    }

    [Fact]
    public void Should_HaveError_When_DescriptionIsEmpty()
    {
        // Arrange
        var task = new Domain.Entities.Task { Description = string.Empty };

        // Act
        var result = _validator.TestValidate(task);

        // Assert
        result.ShouldHaveValidationErrorFor(t => t.Description)
              .WithErrorMessage("Task description is required.");
    }

    [Fact]
    public void Should_HaveError_When_DescriptionExceedsMaxLength()
    {
        // Arrange
        var task = new Domain.Entities.Task { Description = new string('A', 1001) };

        // Act
        var result = _validator.TestValidate(task);

        // Assert
        result.ShouldHaveValidationErrorFor(t => t.Description)
              .WithErrorMessage("Task description must be at most 1000 characters long.");
    }

    [Fact]
    public void Should_HaveError_When_UserIsEmpty()
    {
        // Arrange
        var task = new Domain.Entities.Task { User = string.Empty };

        // Act
        var result = _validator.TestValidate(task);

        // Assert
        result.ShouldHaveValidationErrorFor(t => t.User)
              .WithErrorMessage("User ID is required.");
    }

    [Fact]
    public void Should_HaveError_When_UserExceedsMaxLength()
    {
        // Arrange
        var task = new Domain.Entities.Task { User = new string('A', 101) };

        // Act
        var result = _validator.TestValidate(task);

        // Assert
        result.ShouldHaveValidationErrorFor(t => t.User)
              .WithErrorMessage("User must be at most 100 characters long.");
    }

    [Fact]
    public void Should_HaveError_When_DueDateIsEmpty()
    {
        // Arrange
        var task = new Domain.Entities.Task { DueDate = default };

        // Act
        var result = _validator.TestValidate(task);

        // Assert
        result.ShouldHaveValidationErrorFor(t => t.DueDate)
              .WithErrorMessage("Due date is required.");
    }

    [Fact]
    public void Should_HaveError_When_StatusIsInvalid()
    {
        // Arrange
        var task = new Domain.Entities.Task { Status = (Domain.TaskStatus)999 };

        // Act
        var result = _validator.TestValidate(task);

        // Assert
        result.ShouldHaveValidationErrorFor(t => t.Status)
              .WithErrorMessage("Invalid task status.");
    }

    [Fact]
    public void Should_HaveError_When_PriorityIsInvalid()
    {
        // Arrange
        var task = new Domain.Entities.Task { Priority = (TaskPriority)999 };

        // Act
        var result = _validator.TestValidate(task);

        // Assert
        result.ShouldHaveValidationErrorFor(t => t.Priority)
              .WithErrorMessage("Invalid task priority.");
    }

    [Fact]
    public void Should_NotHaveError_When_AllFieldsAreValid()
    {
        // Arrange
        var task = new Domain.Entities.Task
        {
            Title = "Valid Task",
            Description = "This is a valid task description.",
            User = "ValidUser",
            DueDate = DateTime.UtcNow.AddDays(1),
            Status = Domain.TaskStatus.Pending,
            Priority = TaskPriority.Medium
        };

        // Act
        var result = _validator.TestValidate(task);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}