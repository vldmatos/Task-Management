using Configurations.Data;
using Configurations.Extensions;
using Domain;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace API.Endpoints;

public class Projects : IEndpoint
{
    public string Group => "Projects";

    public void MapEndpoints(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/projects",
            async Task<Ok<Project[]>>
            (DataContext dataContext) =>
            {
                var projects = await dataContext.Projects
                                                .AsNoTracking()
                                                .ToArrayAsync();

                return TypedResults.Ok(projects);
            })
        .RequireRateLimiting(RateLimits.FixedWindow)
        .WithDescription("Get all projects")
        .WithTags(Group)
        .RequireAuthorization(policy => policy.RequireRole
            (Roles.Regular, Roles.Manager));


        endpointRouteBuilder.MapGet("/projects/{id}",
            async Task<Results<Ok<Project>, NotFound>>
            (int id, DataContext dataContext) =>
            {
                var project = await dataContext.Projects
                                               .AsNoTracking()
                                               .FirstOrDefaultAsync(item => item.Id == id);

                return project is not null ?
                    TypedResults.Ok(project) :
                    TypedResults.NotFound();
            })
        .RequireRateLimiting(RateLimits.FixedWindow)
        .WithDescription("Get project by id")
        .WithTags(Group)
        .RequireAuthorization(policy => policy.RequireRole
            (Roles.Regular, Roles.Manager));


        endpointRouteBuilder.MapGet("/projects/{id}/tasks",
            async Task<Results<Ok<Domain.Entities.Task[]>, NotFound>>
            (int id, DataContext dataContext) =>
            {
                var tasks = await dataContext.Tasks
                                             .AsNoTracking()
                                             .Where(item => item.ProjectId == id)
                                             .ToArrayAsync();

                return tasks is not null ?
                    TypedResults.Ok(tasks) :
                    TypedResults.NotFound();
            })
        .RequireRateLimiting(RateLimits.FixedWindow)
        .WithDescription("Get tasks by project id")
        .WithTags(Group)
        .RequireAuthorization(policy => policy.RequireRole
            (Roles.Regular, Roles.Manager));


        endpointRouteBuilder.MapPost("/projects",
            async (DataContext dataContext,
                   HttpContext httpContext,
                   Project project,
                   IValidator<Project> validator) =>
            {
                project.User = httpContext.User.Identity?.Name;

                var validatorResult = validator.Validate(project);
                if (!validatorResult.IsValid)
                    throw new ProblemException("Validation error", validatorResult.Errors);

                dataContext.Projects.Add(project);
                await dataContext.SaveChangesAsync();

                return TypedResults.Created($"/projects/{project.Id}", project);
            })
        .RequireRateLimiting(RateLimits.FixedWindow)
        .WithDescription("Create a new project")
        .WithTags(Group)
        .RequireAuthorization(policy => policy.RequireRole
            (Roles.Regular, Roles.Manager));


        endpointRouteBuilder.MapDelete("/projects/{id}",
            async (int id, DataContext dataContext) =>
            {
                var project = await dataContext.Projects
                                               .Include(p => p.Tasks)
                                               .FirstOrDefaultAsync(p => p.Id == id);

                if (project is null)
                    return Results.NotFound();

                if (!project.CanBeDeleted())
                    throw new ProblemException("Project cannot be deleted",
                                               "Project has pending tasks.");

                dataContext.Projects.Remove(project);
                await dataContext.SaveChangesAsync();

                return Results.NoContent();
            })
        .RequireRateLimiting(RateLimits.FixedWindow)
        .WithDescription("Delete a project by id")
        .WithTags(Group)
        .RequireAuthorization(policy => policy.RequireRole
            (Roles.Regular, Roles.Manager));


        endpointRouteBuilder.MapGet("/projects/{projectId}/report",
            async Task<Results<Ok<Report>, NotFound>>
            (int projectId,
             HttpContext httpContext,
             DataContext dataContext) =>
            {
                var project = await dataContext.Projects
                                               .Include(p => p.Tasks)
                                               .AsNoTracking()
                                               .FirstOrDefaultAsync(p => p.Id == projectId);

                if (project is null)
                    return TypedResults.NotFound();

                var user = new User
                {
                    Username = httpContext.User.Identity?.Name ?? "Unknown",
                    Role = httpContext.User.Claims.FirstOrDefault(c => c.Type == "role")?.Value ?? "Unknown"
                };

                return TypedResults.Ok(project.GenerateReport(user));
            })
        .RequireRateLimiting(RateLimits.FixedWindow)
        .WithDescription("Generate a performance report for a specific project")
        .WithTags(Group)
        .RequireAuthorization(policy => policy.RequireRole(Roles.Manager));
    }
}