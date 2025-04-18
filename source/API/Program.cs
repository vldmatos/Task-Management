using Configurations.Data;
using Configurations.Extensions;
using Scalar.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<DataContext>("projects-database");

builder.Services.AddOpenApi()
                .AddProblemDetail()
                .AddEndpoints(Assembly.GetExecutingAssembly());

builder.Services.AddEndpointsApiExplorer();

var application = builder.Build();

application.MapDefaultEndpoints()
           .MapEndpoints();

application.UseHttpsRedirection()
           .UseExceptionHandler()
           .UseStatusCodePages();

if (application.Environment.IsDevelopment())
{
    application.MapOpenApi();
    application.MapScalarApiReference();
}

application.Run();
