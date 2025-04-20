using Configurations.Authorizations;
using Configurations.Data;
using Configurations.Extensions;
using Domain.Entities;
using FluentValidation;
using Scalar.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();
var key = builder.Configuration["JWT_KEY"] ??
    throw new Exception("JWT_KEY not found in configuration - configure in your secrets");

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<DataContext>("projects-database");

builder.Services.AddOpenApi()
                .AddProblemDetail()
                .AddEndpoints(Assembly.GetExecutingAssembly())
                .AddValidatorsFromAssembly(typeof(Project).Assembly, includeInternalTypes: true);

builder.Services.AddScoped(provider => new TokenService(key));

builder.Services.AddAuthenticationToken(key);
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();

var application = builder.Build();

application.MapDefaultEndpoints()
           .MapEndpoints();

application.UseHttpsRedirection()
           .UseExceptionHandler()
           .UseStatusCodePages();

application.UseAuthentication()
           .UseAuthorization();

if (application.Environment.IsDevelopment())
{
    application.MapOpenApi();
    application.MapScalarApiReference();
}

application.Run();
