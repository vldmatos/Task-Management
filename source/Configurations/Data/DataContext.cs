using Domain;
using Microsoft.EntityFrameworkCore;

namespace Configurations.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<TaskProject> Tasks { get; set; }
}
