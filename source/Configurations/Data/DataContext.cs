using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Configurations.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Project> Projects =>
        Set<Project>();

    public DbSet<TaskProject> Tasks =>
        Set<TaskProject>();
}

public static class Extensions
{
    public static void CreateDbIfNotExists(this IHost host)
    {
        using var scope = host.Services.CreateScope();

        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<DataContext>();
        try
        {
            context.Database.EnsureCreated();
        }
        catch (Exception)
        {
        }
    }
}