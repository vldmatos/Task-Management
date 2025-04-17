using Configurations.Extensions;
using Scalar.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi()
                .AddEndpoints(Assembly.GetExecutingAssembly());

builder.Services.AddEndpointsApiExplorer();

var application = builder.Build();

application.MapDefaultEndpoints()
           .MapEndpoints();

application.UseHttpsRedirection();

if (application.Environment.IsDevelopment())
{
    application.MapOpenApi();
    application.MapScalarApiReference();
}

application.Run();
