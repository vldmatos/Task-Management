using Configurations.Data;
using Configurations.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace API.Endpoints;

public class Tasks : IEndpoint
{
    public string Group => "Tasks";

    public void MapEndpoints(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPut("/tasks/{id}",
            async (int id, Domain.Task task, DataContext dataContext) =>
        {
            var registeredTask = await dataContext.Tasks.FindAsync(id);
            if (registeredTask is null)
                return Results.NotFound();

            registeredTask.Title = task.Title ?? registeredTask.Title;
            registeredTask.Description = task.Description ?? registeredTask.Description;
            registeredTask.DueDate = task.DueDate;

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


        endpointRouteBuilder.MapGet("/tasks/{id}",
            async Task<Results<Ok<Domain.Task>, NotFound>>
            (int id, DataContext dataContext) =>
        {
            var task = await dataContext.Tasks
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(item => item.Id == id);
            return task is not null ?
                TypedResults.Ok(task) :
                TypedResults.NotFound();
        })
        .WithDescription("Get task by id")
        .WithTags(Group);


        endpointRouteBuilder.MapPost("/tasks",
            async (Domain.Task task, DataContext dataContext) =>
        {
            var project = await dataContext.Projects.FindAsync(task.ProjectId);
            if (project is null)
                throw new ProblemException("Project for task not found", "Tasks must be created in an existing project.");

            task.CreatedAt = DateTime.UtcNow;

            await dataContext.Tasks.AddAsync(task);
            await dataContext.SaveChangesAsync();

            return Results.Created($"/tasks/{task.Id}", task);
        })
        .WithDescription("Create a new task in an existing project")
        .WithTags(Group);
    }
}