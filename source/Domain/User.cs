using System.ComponentModel.DataAnnotations;

namespace Domain;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    public UserRole Role { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public required string Email { get; set; }
}