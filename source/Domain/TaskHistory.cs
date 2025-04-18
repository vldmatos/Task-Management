using System.ComponentModel.DataAnnotations;

namespace Domain;

public class TaskHistory
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
    public string FieldChanged { get; set; }

    [Required]
    public string OldValue { get; set; }

    [Required]
    public string NewValue { get; set; }

    [Required]
    public DateTime ChangedAt { get; set; }
}