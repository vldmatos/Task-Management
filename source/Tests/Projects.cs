namespace Tests;

public class Projects
{
    [Fact]
    public void CanAddTaskToProject()
    {
        // Arrange
        var project = new Domain.Project
        {
            Id = 1,
            Name = "Test Project",
            Description = "A project for testing",
            CreatedAt = DateTime.UtcNow,
            User = "User1",
            Tasks = new List<Domain.Task>()
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
        var project = new Domain.Project
        {
            Id = 1,
            Name = "Test Project",
            Description = "A project for testing",
            CreatedAt = DateTime.UtcNow,
            User = "User1",
            Tasks = new List<Domain.Task>()
        };

        // Adiciona 20 tarefas ao projeto
        for (int i = 0; i < Domain.Project.MaxTasks; i++)
        {
            project.Tasks.Add(new Domain.Task
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
}