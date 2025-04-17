namespace Domain
{
    public class TaskProject
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = "Pending";
        public int ProjectId { get; set; }
        public required Project Project { get; set; }
    }
}
