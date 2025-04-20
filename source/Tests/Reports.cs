using Domain;
using Domain.Entities;

namespace Tests;

public class Reports
{
    [Fact]
    public void OnlyManagerCanAccessReports()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Name = "Test Project",
            Description = "A project for testing",
            CreatedAt = DateTime.UtcNow,
            User = "User1",
            Tasks = []
        };

        var managerUser = new User
        {
            Username = "ManagerUser",
            Role = Roles.Manager
        };

        var regularUser = new User
        {
            Username = "RegularUser",
            Role = Roles.Regular
        };

        // Act & Assert
        // Manager should be able to generate the report
        var report = project.GenerateReport(managerUser);
        Assert.NotNull(report);

        // Regular user should not be able to generate the report
        Assert.Throws<UnauthorizedAccessException>(() => project.GenerateReport(regularUser));
    }
}
