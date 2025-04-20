using Configurations.Authorizations;
using Configurations.Data;
using Configurations.Extensions;
using Domain;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace API.Endpoints;

public class Tasks : IEndpoint
{
    public string Group => "Tasks";

    public void MapEndpoints(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("/tasks",
            async (Domain.Entities.Task task,
                   DataContext dataContext,
                   HttpContext httpContext,
                   IValidator<Domain.Entities.Task> validator) =>
            {
                task.User = httpContext.User.Identity?.Name;

                var validatorResult = validator.Validate(task);
                if (!validatorResult.IsValid)
                    throw new ProblemException("Validation error", validatorResult.Errors);

                var project = await dataContext.Projects
                                               .Include(p => p.Tasks)
                                               .FirstOrDefaultAsync(p => p.Id == task.ProjectId) ??
                    throw new ProblemException("Project for task not found",
                                               "Tasks must be created in an existing project.");

                if (!project.CanBeAddTask())
                    throw new ProblemException("Task not can be added",
                                               $"Limit of {Project.MaxTasks} tasks per project.");

                task.CreatedAt = DateTime.UtcNow;

                await dataContext.Tasks.AddAsync(task);
                await dataContext.SaveChangesAsync();

                return Results.Created($"/tasks/{task.Id}", task);
            })
        .WithDescription("Create a new task in an existing project")
        .WithTags(Group)
        .RequireAuthorization(policy => policy.RequireRole
            (Roles.Regular, Roles.Manager));


        endpointRouteBuilder.MapPost("/tasks/comments",
            async (Comment comment,
                   DataContext dataContext,
                   HttpContext httpContext,
                   IValidator<Comment> validator) =>
            {
                comment.User = httpContext.User.Identity?.Name;

                var validatorResult = validator.Validate(comment);
                if (!validatorResult.IsValid)
                    throw new ProblemException("Validation error", validatorResult.Errors);

                var task = await dataContext.Tasks.FindAsync(comment.TaskId) ??
                throw new ProblemException("Task for comment not found",
                                            "Comment must be created in an existing task.");

                comment.CreatedAt = DateTime.UtcNow;

                var taskHistory = task.GetChanges(comment);

                dataContext.TaskHistories.Add(taskHistory);

                await dataContext.Comments.AddAsync(comment);
                await dataContext.SaveChangesAsync();

                return Results.Created($"/tasks/comments/{comment.Id}", comment);
            })
        .WithDescription("Add a comment to a task")
        .WithTags(Group)
        .RequireAuthorization(policy => policy.RequireRole
            (Roles.Regular, Roles.Manager));


        endpointRouteBuilder.MapPut("/tasks/{id}",
            async (int id, Domain.Entities.Task task,
                   DataContext dataContext,
                   HttpContext httpContext,
                   IValidator<Domain.Entities.Task> validator) =>
            {
                task.User = httpContext.User.Identity?.Name;

                var validatorResult = validator.Validate(task);
                if (!validatorResult.IsValid)
                    throw new ProblemException("Validation error", validatorResult.Errors);

                var registeredTask = await dataContext.Tasks.FindAsync(id);
                if (registeredTask is null)
                    return Results.NotFound();

                var taskHistory = registeredTask.GetChanges(task);

                registeredTask = registeredTask.Change(task);

                dataContext.TaskHistories.AddRange(taskHistory);

                await dataContext.SaveChangesAsync();

                return Results.NoContent();
            })
        .WithDescription("Update task by id")
        .WithTags(Group)
        .RequireAuthorization(policy => policy.RequireRole
            (Roles.Regular, Roles.Manager));


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
        .WithTags(Group)
        .RequireAuthorization(policy => policy.RequireRole
            (Roles.Regular, Roles.Manager));


        endpointRouteBuilder.MapGet("/tasks/{id}",
            async Task<Results<Ok<Domain.Entities.Task>, NotFound>>
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
        .WithTags(Group)
        .RequireAuthorization(policy => policy.RequireRole
            (Roles.Regular, Roles.Manager));


        endpointRouteBuilder.MapGet("/tasks/{taskId}/comments",
            async Task<Results<Ok<List<Comment>>, NotFound>>
            (int taskId, DataContext dataContext) =>
            {
                var task = await dataContext.Tasks
                                            .Include(t => t.Comments)
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(t => t.Id == taskId);

                if (task is null)
                    return TypedResults.NotFound();

                var comments = task.Comments.ToList();
                return TypedResults.Ok(comments);
            })
        .WithDescription("Get all comments for a specific task by task ID")
        .WithTags(Group)
        .RequireAuthorization(policy => policy.RequireRole(Roles.Regular, Roles.Manager));


        endpointRouteBuilder.MapGet("/tasks/{taskId}/histories",
            async Task<Results<Ok<List<TaskHistory>>, NotFound>>
            (int taskId, DataContext dataContext) =>
            {
                var task = await dataContext.Tasks
                                            .Include(t => t.HistoryEntries)
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(t => t.Id == taskId);

                if (task is null)
                    return TypedResults.NotFound();

                var histories = task.HistoryEntries.ToList();
                return TypedResults.Ok(histories);
            })
        .WithDescription("Get all task histories for a specific task by task ID")
        .WithTags(Group)
        .RequireAuthorization(policy => policy.RequireRole(Roles.Regular, Roles.Manager));
    }
}