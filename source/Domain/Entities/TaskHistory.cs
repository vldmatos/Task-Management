﻿using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class TaskHistory
{
    [Key]
    public int Id { get; set; }

    public int TaskId { get; set; }

    public string User { get; set; }

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

        RuleFor(x => x.User)
            .NotEmpty()
            .WithMessage("User ID is required.")
            .MaximumLength(100)
            .WithMessage("User must be at most 100 characters long.");

        RuleFor(x => x.Change)
            .NotEmpty()
            .WithMessage("Field change is required.")
            .MaximumLength(1000)
            .WithMessage("Field changed must be at most 1000 characters long.");
    }
}