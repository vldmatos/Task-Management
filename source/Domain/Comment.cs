using System.ComponentModel.DataAnnotations;

namespace Domain;


public class Comment
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int TaskId { get; set; }

    public virtual Task Task { get; set; }

    [Required]
    public int UserId { get; set; }

    public virtual User User { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Content { get; set; }

    public DateTime CreatedAt { get; set; }

    public Comment()
    {
        CreatedAt = DateTime.Now;
    }
}
