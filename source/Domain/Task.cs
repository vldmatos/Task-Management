using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Task
{
    [Key]
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime DueDate { get; set; }

    public TaskStatus Status { get; set; }

    public TaskPriority Priority { get; init; }

    public DateTime CreatedAt { get; set; }

    public int ProjectId { get; set; }

    public virtual ICollection<TaskHistory> HistoryEntries { get; set; }

    public virtual ICollection<Comment> Comments { get; set; }


    public Task Change(Task task)
    {
        ArgumentNullException.ThrowIfNull(task);

        if (task.Title != Title)
            Title = task.Title;

        if (task.Description != Description)
            Description = task.Description;

        if (task.DueDate != DueDate)
            DueDate = task.DueDate;

        if (task.Status != Status)
            Status = task.Status;

        return this;
    }

    public List<TaskHistory> GetChanges(Task task)
    {
        ArgumentNullException.ThrowIfNull(task);

        var changes = new List<TaskHistory>();

        if (task.Title != Title)
            changes.Add(new TaskHistory { TaskId = Id, Change = $"Title changed to: {task.Title}", ChangedAt = DateTime.UtcNow });

        if (task.Description != Description)
            changes.Add(new TaskHistory { TaskId = Id, Change = $"Description changed to: {task.Description}", ChangedAt = DateTime.UtcNow });

        if (task.DueDate != DueDate)
            changes.Add(new TaskHistory { TaskId = Id, Change = $"DueDate changed to: {task.DueDate}", ChangedAt = DateTime.UtcNow });

        if (task.Status != Status)
            changes.Add(new TaskHistory { TaskId = Id, Change = $"Status changed to: {task.Status}", ChangedAt = DateTime.UtcNow });

        return changes;
    }

    public TaskHistory GetChanges(Comment comment)
    {
        ArgumentNullException.ThrowIfNull(comment);

        return new TaskHistory
        {
            TaskId = Id,
            Change = $"Comment added: {comment.Content}",
            ChangedAt = DateTime.UtcNow
        };
    }
}

public sealed class TaskValidator : AbstractValidator<Task>
{
    public TaskValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Task title is required.")
            .MaximumLength(100)
            .WithMessage("Task title must be at most 100 characters long.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Task description is required.")
            .MaximumLength(1000)
            .WithMessage("Task description must be at most 1000 characters long.");

        RuleFor(x => x.DueDate)
            .NotEmpty()
            .WithMessage("Due date is required.");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Invalid task status.");

        RuleFor(x => x.Priority)
            .IsInEnum()
            .WithMessage("Invalid task priority.");
    }
}