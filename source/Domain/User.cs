using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Domain;

public class User
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    public UserRole Role { get; set; }

    public string Email { get; set; }
}

public sealed class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("User name is required.")
            .MaximumLength(100)
            .WithMessage("User name must be at most 100 characters long.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.");

        RuleFor(x => x.Role)
            .IsInEnum()
            .WithMessage("Invalid user role.");
    }
}