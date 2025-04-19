using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Domain;

public class TaskHistory
{
    [Key]
    public int Id { get; set; }

    public int TaskId { get; set; }

    public int UserId { get; set; }

    public string Change { get; set; }

    public DateTime ChangedAt { get; set; }
}

public sealed class TasksHistoryValidator : AbstractValidator<TaskHistory>
{
    public TasksHistoryValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEmpty()
            .WithMessage("Task ID is required.")
            .GreaterThan(0)
            .WithMessage("Task ID must be greater than 0.");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.")
            .GreaterThan(0)
            .WithMessage("User ID must be greater than 0.");

        RuleFor(x => x.Change)
            .NotEmpty()
            .WithMessage("Field change is required.")
            .MaximumLength(1000)
            .WithMessage("Field changed must be at most 1000 characters long.");
    }
}