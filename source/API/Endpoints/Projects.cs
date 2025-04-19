using Configurations.Data;
using Configurations.Extensions;
using Domain;
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
        .WithDescription("Get all projects")
        .WithTags(Group);


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
        .WithDescription("Get project by id")
        .WithTags(Group);


        endpointRouteBuilder.MapGet("/projects/{id}/tasks",
            async Task<Results<Ok<Domain.Task[]>, NotFound>>
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
        .WithDescription("Get tasks by project id")
        .WithTags(Group);


        endpointRouteBuilder.MapPost("/projects",
            async (DataContext dataContext, Project project, IValidator<Project> validator) =>
            {
                var validatorResult = validator.Validate(project);
                if (!validatorResult.IsValid)
                    throw new ProblemException("Validation error", validatorResult.Errors);

                dataContext.Projects.Add(project);
                await dataContext.SaveChangesAsync();

                return TypedResults.Created($"/projects/{project.Id}", project);
            })
        .WithDescription("Create a new project")
        .WithTags(Group);


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
        .WithDescription("Delete a project by id")
        .WithTags(Group);
    }
}