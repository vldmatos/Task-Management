﻿using Configurations.Data;
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
            async Task<Results<Ok<Domain.Task>, NotFound>>
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

            return TypedResults.Created($"/projects/{project.Id}", project);
        })
        .WithDescription("Create a new project")
        .WithTags(Group);


        endpointRouteBuilder.MapDelete("/projects/{projectId}/tasks/{taskId}",
            async Task<Results<NoContent, NotFound>>
            (int projectId, int taskId, DataContext dataContext) =>
        {
            var task = await dataContext.Tasks
                                        .FirstOrDefaultAsync(t => t.Id == taskId &&
                                                                  t.ProjectId == projectId)
                       ??
                       throw new ProblemException("Task not found",
                                                  "The task you are trying to remove does not exist or is not part of the specified project.");

            dataContext.Tasks.Remove(task);
            await dataContext.SaveChangesAsync();

            return TypedResults.NoContent();
        })
        .WithDescription("Remove a task from a project")
        .WithTags(Group);
    }
}