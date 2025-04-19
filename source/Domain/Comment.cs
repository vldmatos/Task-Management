using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Comment
{
    [Key]
    public int Id { get; set; }

    public int TaskId { get; set; }

    public int UserId { get; set; }

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

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.")
            .GreaterThan(0)
            .WithMessage("User ID must be greater than 0.");

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Comment content is required.")
            .MaximumLength(500)
            .WithMessage("Comment content must be at most 500 characters long.");
    }
}