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
            .MaximumLength(500)
            .WithMessage("Task description must be at most 500 characters long.");

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