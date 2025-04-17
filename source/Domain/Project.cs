namespace Domain
{
    public class Project
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<TaskProject> Tasks { get; set; } = [];
    }
}
