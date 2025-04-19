using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public required string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        [Required]
        public int UserId { get; set; }

        public virtual ICollection<Task> Tasks { get; set; } = [];
    }
}
