using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Project
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = [];

    public bool CanBeDeleted()
    {
        if (Tasks.Count == 0)
            return true;

        foreach (var task in Tasks)
        {
            if (task.Status == TaskStatus.Pending)
                return false;
        }

        return true;
    }
}

public sealed class ProjectValidator : AbstractValidator<Project>
{
    public ProjectValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Project name is required.")
            .MaximumLength(100)
            .WithMessage("Project name must be at most 100 characters long.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Project description is required.")
            .MaximumLength(500)
            .WithMessage("Project description must be at most 500 characters long.");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.")
            .GreaterThan(0)
            .WithMessage("User ID must be greater than 0.");
    }
}
