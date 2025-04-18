using Configurations.Data;
using Configurations.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace API.Endpoints;

public class User : IEndpoint
{
    public string Group => "User";

    public void MapEndpoints(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/user/{id}/projects",
           async Task<Results<Ok<Domain.Project>, NotFound>>
           (int id, DataContext dataContext) =>
        {
            var projects = await dataContext.Projects
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(item => item.UserId == id);

            return projects is not null ?
                TypedResults.Ok(projects) :
                TypedResults.NotFound();
        })
       .WithDescription("Get projects by user")
       .WithTags(Group);

    }
}
