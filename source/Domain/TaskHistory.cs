using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Domain;

public class TaskHistory
{
    [Key]
    public int Id { get; set; }

    public int TaskId { get; set; }

    public int UserId { get; set; }

    public string FieldChanged { get; set; }

    public string OldValue { get; set; }

    public string NewValue { get; set; }

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

        RuleFor(x => x.FieldChanged)
            .NotEmpty()
            .WithMessage("Field changed is required.")
            .MaximumLength(100)
            .WithMessage("Field changed must be at most 100 characters long.");

        RuleFor(x => x.OldValue)
            .NotEmpty()
            .WithMessage("Old value is required.")
            .MaximumLength(500)
            .WithMessage("Old value must be at most 500 characters long.");

        RuleFor(x => x.NewValue)
            .NotEmpty()
            .WithMessage("New value is required.")
            .MaximumLength(500)
            .WithMessage("New value must be at most 500 characters long.");
    }
}