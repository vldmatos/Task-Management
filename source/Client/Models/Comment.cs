namespace Client.Models;

public class Comment
{
    public int Id { get; set; }

    public int TaskId { get; set; }

    public string? User { get; set; }

    public string? Content { get; set; }

    public DateTime CreatedAt { get; set; }
}
