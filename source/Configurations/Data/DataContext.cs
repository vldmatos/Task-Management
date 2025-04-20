using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Configurations.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<Domain.Entities.Task> Tasks { get; set; }
    public DbSet<TaskHistory> TaskHistories { get; set; }
    public DbSet<Comment> Comments { get; set; }

    private const string DateTimeType = "timestamp with time zone";

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
                  .HasColumnType(DateTimeType);
        });

        modelBuilder.Entity<Domain.Entities.Task>(entity =>
        {
            entity.HasKey(options => options.Id);
            entity.Property(options => options.Id)
                  .UseIdentityAlwaysColumn()
                  .ValueGeneratedOnAdd();

            entity.Property(options => options.Title)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(options => options.User)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(options => options.Description)
                  .HasMaxLength(1000);

            entity.Property(options => options.DueDate)
                  .IsRequired()
                  .HasColumnType(DateTimeType);

            entity.Property(options => options.CreatedAt)
                  .HasColumnType(DateTimeType);

            entity.Property(options => options.Status)
                  .IsRequired()
                  .HasConversion<int>();

            entity.Property(options => options.Priority)
                  .IsRequired()
                  .HasConversion<int>();
        });

        modelBuilder.Entity<TaskHistory>(entity =>
        {
            entity.HasKey(options => options.Id);
            entity.Property(options => options.Id)
                  .UseIdentityAlwaysColumn()
                  .ValueGeneratedOnAdd();

            entity.Property(options => options.Change)
                  .IsRequired()
                  .HasMaxLength(1000);

            entity.Property(options => options.ChangedAt)
                  .IsRequired()
                  .HasColumnType(DateTimeType);
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
                  .HasColumnType(DateTimeType);
        });
    }
}