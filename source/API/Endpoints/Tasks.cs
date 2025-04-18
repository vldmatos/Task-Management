using Configurations.Data;
using Configurations.Extensions;
using Domain;

namespace API.Endpoints;

public class Tasks : IEndpoint
{
    public string Group => "Tasks";

    public void MapEndpoints(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPut("/tasks/{id}",
            async (int id, TaskProject taskProject, DataContext dataContext) =>
        {
            var registeredTask = await dataContext.Tasks.FindAsync(id);
            if (registeredTask is null)
                return Results.NotFound();

            registeredTask.Title = taskProject.Title ?? registeredTask.Title;
            registeredTask.Description = taskProject.Description ?? registeredTask.Description;
            registeredTask.Status = taskProject.Status ?? registeredTask.Status;
            registeredTask.DueDate = taskProject.DueDate;

            await dataContext.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithDescription("Update task by id")
        .WithTags(Group);


        endpointRouteBuilder.MapDelete("/tasks/{id}",
            async (int id, DataContext dataContext) =>
        {
            var task = await dataContext.Tasks.FindAsync(id);
            if (task is null)
                return Results.NotFound();

            dataContext.Tasks.Remove(task);
            await dataContext.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithDescription("Delete task by id")
        .WithTags(Group);
    }
}
