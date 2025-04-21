using Configurations.Authorizations;
using Configurations.Extensions;
using Domain;

namespace API.Endpoints;

public class Account : IEndpoint
{
    public string Group => "Account";

    public void MapEndpoints(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("/account/login/regular",
            async (HttpContext context, TokenService tokenService) =>
            {
                var user = new User
                {
                    Username = "regular-user@teste.com",
                    Role = Roles.Regular
                };

                var token = tokenService.GenerateToken(user);


                await context.Response.WriteAsync($"Logged in as Regular User - Bearer: {token}");
            })
        .WithDescription("Simulate login as Regular user")
        .WithTags(Group)
        .AllowAnonymous();


        endpointRouteBuilder.MapPost("/account/login/manager",
            async (HttpContext context, TokenService tokenService) =>
            {
                var user = new User
                {
                    Username = "manager-user@teste.com",
                    Role = Roles.Manager
                };

                var token = tokenService.GenerateToken(user);


                await context.Response.WriteAsync($"Logged in as Manager User - Bearer: {token}");
            })
        .RequireRateLimiting(RateLimits.FixedWindow)
        .WithDescription("Simulate login as Manager user")
        .WithTags(Group)
        .AllowAnonymous();
    }
}