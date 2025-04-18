using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Configurations.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Domain.Task> Tasks { get; set; }
    public DbSet<TaskHistory> TaskHistories { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(options => options.Id);
            entity.Property(options => options.Id)
                  .UseIdentityAlwaysColumn()
                  .ValueGeneratedOnAdd();

            entity.Property(options => options.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(options => options.Email)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(options => options.Role)
                  .IsRequired()
                  .HasConversion<int>();
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(options => options.Id);
            entity.Property(options => options.Id)
                  .UseIdentityAlwaysColumn()
                  .ValueGeneratedOnAdd();

            entity.Property(options => options.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(options => options.Description)
                  .HasMaxLength(500);

            entity.Property(options => options.CreatedAt)
                .HasColumnType("timestamp with time zone");

            entity.HasOne(options => options.User)
                  .WithMany(u => u.Projects)
                  .HasForeignKey(options => options.UserId)
                  .OnDelete(DeleteBehavior.Cascade)
                  .IsRequired();
        });

        modelBuilder.Entity<Domain.Task>(entity =>
        {
            entity.HasKey(options => options.Id);
            entity.Property(options => options.Id)
                  .UseIdentityAlwaysColumn()
                  .ValueGeneratedOnAdd();

            entity.Property(options => options.Title)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(options => options.Description)
                  .HasMaxLength(1000);

            entity.Property(options => options.DueDate)
                  .IsRequired()
                  .HasColumnType("timestamp with time zone");

            entity.Property(options => options.CreatedAt)
                  .HasColumnType("timestamp with time zone");

            entity.Property(options => options.Status)
                  .IsRequired()
                  .HasConversion<int>();

            entity.Property(options => options.Priority)
                  .IsRequired()
                  .HasConversion<int>();

            entity.HasOne(options => options.Project)
                  .WithMany(p => p.Tasks)
                  .HasForeignKey(options => options.ProjectId)
                  .OnDelete(DeleteBehavior.Cascade)
                  .IsRequired();
        });

        modelBuilder.Entity<TaskHistory>(entity =>
        {
            entity.HasKey(options => options.Id);
            entity.Property(options => options.Id)
                  .UseIdentityAlwaysColumn()
                  .ValueGeneratedOnAdd();

            entity.Property(options => options.FieldChanged)
                  .IsRequired();

            entity.Property(options => options.OldValue)
                  .IsRequired();

            entity.Property(options => options.NewValue)
                  .IsRequired();

            entity.Property(options => options.ChangedAt)
                  .IsRequired()
                  .HasColumnType("timestamp with time zone");

            entity.HasOne(options => options.Task)
                  .WithMany(t => t.HistoryEntries)
                  .HasForeignKey(options => options.TaskId)
                  .OnDelete(DeleteBehavior.Cascade)
                  .IsRequired();

            entity.HasOne(options => options.User)
                  .WithMany()
                  .HasForeignKey(options => options.UserId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .IsRequired();
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(options => options.Id);
            entity.Property(options => options.Id)
                  .UseIdentityAlwaysColumn()
                  .ValueGeneratedOnAdd();

            entity.Property(options => options.Content)
                  .IsRequired()
                  .HasMaxLength(1000);

            entity.Property(options => options.CreatedAt)
                  .HasColumnType("timestamp with time zone");

            entity.HasOne(options => options.Task)
                  .WithMany(t => t.Comments)
                  .HasForeignKey(options => options.TaskId)
                  .OnDelete(DeleteBehavior.Cascade)
                  .IsRequired();

            entity.HasOne(options => options.User)
                  .WithMany()
                  .HasForeignKey(options => options.UserId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .IsRequired();
        });
    }
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