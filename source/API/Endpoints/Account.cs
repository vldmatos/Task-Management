using Configurations.Authorizations;
using Configurations.Extensions;
using Domain;

namespace API.Endpoints;

public class Account : IEndpoint
{
    public string Group => "Account";

    public void MapEndpoints(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/account/login/regular",
            async (HttpContext context, TokenService tokenService) =>
            {
                var user = new User
                {
                    Username = "regular-user@teste.com",
                    Role = Roles.Regular
                };

                await context.Response.WriteAsync(tokenService.GenerateToken(user));
            })
        .WithDescription("Simulate login as Regular user")
        .WithTags(Group)
        .AllowAnonymous();


        endpointRouteBuilder.MapGet("/account/login/manager",
            async (HttpContext context, TokenService tokenService) =>
            {
                var user = new User
                {
                    Username = "manager-user@teste.com",
                    Role = Roles.Manager
                };

                await context.Response.WriteAsync(tokenService.GenerateToken(user));
            })
        .RequireRateLimiting(RateLimits.FixedWindow)
        .WithDescription("Simulate login as Manager user")
        .WithTags(Group)
        .AllowAnonymous();
    }
}