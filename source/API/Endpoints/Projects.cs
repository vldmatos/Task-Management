using Configurations.Data;
using Configurations.Extensions;
using Domain;

namespace API.Endpoints;

public class Projects : IEndpoint
{
    public string Group => "projects";

    public void MapEndpoints(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet
        ("/projects", () =>

            "Projects!"
        )
        .WithTags(Group)
        .WithOpenApi();

        endpointRouteBuilder.MapPost("/projects", async (DataContext dataContext, Project newProject) =>
        {
            dataContext.Projects.Add(newProject);
            await dataContext.SaveChangesAsync();
            return Results.Created($"/projects/{newProject.Id}", newProject);
        })
        .WithTags(Group)
        .WithOpenApi();
    }
}