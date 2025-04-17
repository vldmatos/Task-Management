namespace Domain
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<TaskProject> Tasks { get; set; } = [];
    }
}
