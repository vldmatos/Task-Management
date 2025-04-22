namespace Web.Models;

public class Report
{
    public int? ProjectId { get; set; }
    public string? ProjectName { get; set; }
    public int? TotalTasks { get; set; }
    public int? CompletedTasks { get; set; }
    public int? PendingTasks { get; set; }
    public double? AverageCompletionTime { get; set; }
    public short? DaysCompletedTasksPerUser { get; set; }
    public double? AverageDaysCompletedTasksPerUser { get; set; }
}
