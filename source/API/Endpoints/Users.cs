using Configurations.Data;
using Configurations.Extensions;
using Domain;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace API.Endpoints;

public class Users : IEndpoint
{
    public string Group => "Users";

    public void MapEndpoints(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/users/{id}",
           async Task<Results<Ok<Domain.User>, NotFound>>
           (int id, DataContext dataContext) =>
            {
                var user = await dataContext.Users
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(user => user.Id == id);

                return user is not null ?
                    TypedResults.Ok(user) :
                    TypedResults.NotFound();
            })
        .WithDescription("Get user by ID")
        .WithTags(Group);


        endpointRouteBuilder.MapPost("/users",
           async Task<Results<Created<User>, BadRequest>>
           (User user, DataContext dataContext, IValidator<User> validator) =>
           {
               var validatorResult = validator.Validate(user);
               if (!validatorResult.IsValid)
                   throw new ProblemException("Validation error", validatorResult.Errors);

               dataContext.Users.Add(user);
               await dataContext.SaveChangesAsync();

               return TypedResults.Created($"/users/{user.Id}", user);
           })
        .WithDescription("Create a new user")
        .WithTags(Group);


        endpointRouteBuilder.MapGet("/users/{id}/projects",
           async Task<Results<Ok<Project>, NotFound>>
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
