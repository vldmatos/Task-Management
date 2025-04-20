using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Project
{
    public const short MaxTasks = 20;
    public const short DaysAvaragePerUser = 30;

    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public string User { get; set; }

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

    public bool CanBeAddTask()
    {
        return Tasks.Count < MaxTasks;
    }

    public Report GenerateReport(User user)
    {
        if (user.Role != Roles.Manager)
            throw new UnauthorizedAccessException("User does not have permission to generate a report.");

        var report = new Report
        (
            Id,
            Name,
            Tasks.Count,
            Tasks.Count(t => t.Status == TaskStatus.Completed),
            Tasks.Count(t => t.Status == TaskStatus.Pending),
            Tasks.Where(t => t.Status == TaskStatus.Completed)
                 .Select(t => (t.CreatedAt - t.DueDate).TotalDays)
                 .DefaultIfEmpty(0)
                 .Average(),
            DaysAvaragePerUser,
            CalculateAverageCompletedTasksPerUser()
        );

        return report;
    }

    private double CalculateAverageCompletedTasksPerUser()
    {
        var now = DateTime.UtcNow;
        var thirtyDaysAgo = now.AddDays(-DaysAvaragePerUser);

        var completedTasksLastDays = Tasks.Where(t =>
                                                 t.Status == TaskStatus.Completed &&
                                                 t.CreatedAt >= thirtyDaysAgo);

        var averageCompletedTasksPerUser = completedTasksLastDays
                                                .GroupBy(t => t.User)
                                                .Select(g => g.Count())
                                                .DefaultIfEmpty(0)
                                                .Average();

        return averageCompletedTasksPerUser;
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

        RuleFor(x => x.User)
            .NotEmpty()
            .WithMessage("User is required.")
            .MaximumLength(100)
            .WithMessage("User must be at most 100 characters long.");
    }
}
