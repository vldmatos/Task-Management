using Domain.Entities;
using FluentValidation.TestHelper;

namespace Tests;

public class Projects
{
    private readonly ProjectValidator _validator;

    public Projects()
    {
        _validator = new Domain.Entities.ProjectValidator();
    }

    [Fact]
    public void CanAddTaskToProject()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Name = "Test Project",
            Description = "A project for testing",
            CreatedAt = DateTime.UtcNow,
            User = "User1",
            Tasks = []
        };

        // Act
        var canAddTask = project.CanBeAddTask();

        // Assert
        Assert.True(canAddTask, "Project should allow adding tasks.");
    }

    [Fact]
    public void CannotAddMoreThan20TasksToProject()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Name = "Test Project",
            Description = "A project for testing",
            CreatedAt = DateTime.UtcNow,
            User = "User1",
            Tasks = []
        };

        // Adiciona 20 tarefas ao projeto
        for (int i = 0; i < Project.MaxTasks; i++)
        {
            project.Tasks.Add(new Domain.Entities.Task
            {
                Id = i + 1,
                Title = $"Task {i + 1}",
                Description = "Test Task",
                DueDate = DateTime.UtcNow.AddDays(5),
                Status = Domain.TaskStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                User = "User1",
                ProjectId = project.Id
            });
        }

        // Act
        var canAddTask = project.CanBeAddTask();

        // Assert
        Assert.False(canAddTask, "Project should not allow adding more than 20 tasks.");
    }

    [Fact]
    public void CanBeDeleted_WhenNoTasks_ReturnsTrue()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Name = "Test Project",
            Description = "A project for testing",
            CreatedAt = DateTime.UtcNow,
            User = "User1",
            Tasks = []
        };

        // Act
        var canBeDeleted = project.CanBeDeleted();

        // Assert
        Assert.True(canBeDeleted, "Project with no tasks should be deletable.");
    }

    [Fact]
    public void CanBeDeleted_WhenAllTasksCompleted_ReturnsTrue()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Name = "Test Project",
            Description = "A project for testing",
            CreatedAt = DateTime.UtcNow,
            User = "User1",
            Tasks =
            [
                new() { Id = 1, Status = Domain.TaskStatus.Completed },
                new() { Id = 2, Status = Domain.TaskStatus.Completed }
            ]
        };

        // Act
        var canBeDeleted = project.CanBeDeleted();

        // Assert
        Assert.True(canBeDeleted, "Project with all tasks completed should be deletable.");
    }

    [Fact]
    public void CanBeDeleted_WhenPendingTasksExist_ReturnsFalse()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Name = "Test Project",
            Description = "A project for testing",
            CreatedAt = DateTime.UtcNow,
            User = "User1",
            Tasks =
            [
                new() { Id = 1, Status = Domain.TaskStatus.Completed },
                new() { Id = 2, Status = Domain.TaskStatus.Pending }
            ]
        };

        // Act
        var canBeDeleted = project.CanBeDeleted();

        // Assert
        Assert.False(canBeDeleted, "Project with pending tasks should not be deletable.");
    }

    [Fact]
    public void CalculateAverageCompletedTasksPerUser_WhenNoTasks_ReturnsZero()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Name = "Test Project",
            Description = "A project for testing",
            CreatedAt = DateTime.UtcNow,
            User = "User1",
            Tasks = []
        };

        // Act
        var average = project.CalculateAverageCompletedTasksPerUser();

        // Assert
        Assert.Equal(0, average);
    }

    [Fact]
    public void CalculateAverageCompletedTasksPerUser_WhenTasksOutside30Days_ReturnsZero()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Name = "Test Project",
            Description = "A project for testing",
            CreatedAt = DateTime.UtcNow,
            User = "User1",
            Tasks =
            [
                new() { Id = 1, Status = Domain.TaskStatus.Completed, CreatedAt = DateTime.UtcNow.AddDays(-31), User = "User1" },
                new() { Id = 2, Status = Domain.TaskStatus.Completed, CreatedAt = DateTime.UtcNow.AddDays(-40), User = "User2" }
            ]
        };

        // Act
        var average = project.CalculateAverageCompletedTasksPerUser();

        // Assert
        Assert.Equal(0, average);
    }

    [Fact]
    public void CalculateAverageCompletedTasksPerUser_WhenTasksWithin30Days_ReturnsCorrectAverage()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Name = "Test Project",
            Description = "A project for testing",
            CreatedAt = DateTime.UtcNow,
            User = "User1",
            Tasks =
            [
                new() { Id = 1, Status = Domain.TaskStatus.Completed, CreatedAt = DateTime.UtcNow.AddDays(-10), User = "User1" },
                new() { Id = 2, Status = Domain.TaskStatus.Completed, CreatedAt = DateTime.UtcNow.AddDays(-5), User = "User1" },
                new() { Id = 3, Status = Domain.TaskStatus.Completed, CreatedAt = DateTime.UtcNow.AddDays(-15), User = "User2" }
            ]
        };

        // Act
        var average = project.CalculateAverageCompletedTasksPerUser();

        // Assert
        Assert.Equal(1.5, average); // User1 completed 2 tasks, User2 completed 1 task, average = (2 + 1) / 2 = 1.5
    }

    [Fact]
    public void CalculateAverageCompletedTasksPerUser_WhenNoCompletedTasksWithin30Days_ReturnsZero()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Name = "Test Project",
            Description = "A project for testing",
            CreatedAt = DateTime.UtcNow,
            User = "User1",
            Tasks =
            [
                new() { Id = 1, Status = Domain.TaskStatus.Pending, CreatedAt = DateTime.UtcNow.AddDays(-10), User = "User1" },
                new() { Id = 2, Status = Domain.TaskStatus.InProgress, CreatedAt = DateTime.UtcNow.AddDays(-5), User = "User2" }
            ]
        };

        // Act
        var average = project.CalculateAverageCompletedTasksPerUser();

        // Assert
        Assert.Equal(0, average);
    }

    [Fact]
    public void Should_HaveError_When_NameIsEmpty()
    {
        // Arrange
        var project = new Project { Name = string.Empty };

        // Act
        var result = _validator.TestValidate(project);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Name)
              .WithErrorMessage("Project name is required.");
    }

    [Fact]
    public void Should_HaveError_When_NameExceedsMaxLength()
    {
        // Arrange
        var project = new Project { Name = new string('A', 101) };

        // Act
        var result = _validator.TestValidate(project);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Name)
              .WithErrorMessage("Project name must be at most 100 characters long.");
    }

    [Fact]
    public void Should_HaveError_When_DescriptionIsEmpty()
    {
        // Arrange
        var project = new Project { Description = string.Empty };

        // Act
        var result = _validator.TestValidate(project);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Description)
              .WithErrorMessage("Project description is required.");
    }

    [Fact]
    public void Should_HaveError_When_DescriptionExceedsMaxLength()
    {
        // Arrange
        var project = new Project { Description = new string('A', 501) };

        // Act
        var result = _validator.TestValidate(project);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Description)
              .WithErrorMessage("Project description must be at most 500 characters long.");
    }

    [Fact]
    public void Should_HaveError_When_UserIsEmpty()
    {
        // Arrange
        var project = new Project { User = string.Empty };

        // Act
        var result = _validator.TestValidate(project);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.User)
              .WithErrorMessage("User is required.");
    }

    [Fact]
    public void Should_HaveError_When_UserExceedsMaxLength()
    {
        // Arrange
        var project = new Project { User = new string('A', 101) };

        // Act
        var result = _validator.TestValidate(project);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.User)
              .WithErrorMessage("User must be at most 100 characters long.");
    }

    [Fact]
    public void Should_NotHaveError_When_AllFieldsAreValid()
    {
        // Arrange
        var project = new Project
        {
            Name = "Valid Project",
            Description = "This is a valid project description.",
            User = "ValidUser"
        };

        // Act
        var result = _validator.TestValidate(project);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}