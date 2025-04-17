using Configurations.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Infrastructure;

public class Worker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource ActivitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = serviceProvider.CreateScope();
            var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();

            await RunMigrationAsync(dataContext, cancellationToken);
            await SeedDataAsync(dataContext, cancellationToken);
        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            throw;
        }

        hostApplicationLifetime.StopApplication();
    }

    private static async Task RunMigrationAsync(DataContext dataContext, CancellationToken cancellationToken)
    {
        var strategy = dataContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await dataContext.Database.MigrateAsync(cancellationToken);
        });
    }

    private static async Task SeedDataAsync(DataContext dataContext, CancellationToken cancellationToken)
    {
        var strategy = dataContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dataContext.Database.BeginTransactionAsync(cancellationToken);

            await dataContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }
}