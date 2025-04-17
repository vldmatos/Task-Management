using Configurations.Extensions;

namespace API.Endpoints;

public class Projects : IEndpoint
{
    public string Group => "projects";

    public void MapEndpoints(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/projects", () =>

            "Projects!"
        )
        .WithTags(Group)
        .WithOpenApi();
    }
}
