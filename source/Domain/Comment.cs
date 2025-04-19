using System.ComponentModel.DataAnnotations;

namespace Domain;


public class Comment
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int TaskId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Content { get; set; }

    public DateTime CreatedAt { get; set; }
}
