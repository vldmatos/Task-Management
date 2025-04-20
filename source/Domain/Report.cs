namespace Domain;

public record Report
(
    int ProjectId,
    string ProjectName,
    int TotalTasks,
    int CompletedTasks,
    int PendingTasks,
    double AverageCompletionTime
);
