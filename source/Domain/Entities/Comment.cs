using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Comment
{
    [Key]
    public int Id { get; set; }

    public int TaskId { get; set; }

    public string User { get; set; }

    public string Content { get; set; }

    public DateTime CreatedAt { get; set; }
}

public sealed class CommentValidator : AbstractValidator<Comment>
{
    public CommentValidator()
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

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Comment content is required.")
            .MaximumLength(1000)
            .WithMessage("Comment content must be at most 1000 characters long.");
    }
}