namespace Tests;

public class Tasks
{
    [Fact]
    public void Change_ShouldRecordChangesInHistory()
    {
        // Arrange
        var task = new Domain.Task
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

        var updatedTask = new Domain.Task
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
}