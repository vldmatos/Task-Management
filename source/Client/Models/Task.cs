namespace Client.Models;

public class Task
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime DueDate { get; set; }

    public TaskStatus Status { get; set; }

    public TaskPriority Priority { get; init; }

    public DateTime CreatedAt { get; set; }

    public string? User { get; set; }

    public int ProjectId { get; set; }

    public Comment[] Comments { get; set; } = [];
}
