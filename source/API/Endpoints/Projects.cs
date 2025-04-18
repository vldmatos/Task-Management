using Configurations.Data;
using Configurations.Extensions;
using Domain;
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
            async Task<Results<Ok<TaskProject>, NotFound>>
            (int id, DataContext dataContext) =>
        {
            var tasks = await dataContext.Tasks
                                           .AsNoTracking()
                                           .FirstOrDefaultAsync(item => item.ProjectId == id);

            return tasks is not null ?
                TypedResults.Ok(tasks) :
                TypedResults.NotFound();
        })
        .WithDescription("Get tasks by project id")
        .WithTags(Group);


        endpointRouteBuilder.MapPost("/projects",
            async (DataContext dataContext, Project project) =>
        {
            dataContext.Projects.Add(project);
            await dataContext.SaveChangesAsync();

            return TypedResults.Created();
        })
        .WithDescription("Create a new project")
        .WithTags(Group);
    }
}