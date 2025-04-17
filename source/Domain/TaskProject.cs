namespace Domain
{
    public class TaskProject
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = "Pending";
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;
    }
}
