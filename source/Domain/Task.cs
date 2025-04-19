using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Task
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public TaskStatus Status { get; set; }

        [Required]
        public TaskPriority Priority { get; init; }

        public DateTime CreatedAt { get; set; }

        [Required]
        public int ProjectId { get; set; }

        public virtual ICollection<TaskHistory> HistoryEntries { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
