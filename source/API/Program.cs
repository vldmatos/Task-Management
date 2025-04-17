using Configurations.Data;
using Configurations.Extensions;
using Microsoft.AspNetCore.Http.Json;
using Scalar.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<DataContext>("database");
builder.Services.Configure<JsonOptions>(options => options.SerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddOpenApi()
                .AddProblemDetail()
                .AddEndpoints(Assembly.GetExecutingAssembly());

builder.Services.AddEndpointsApiExplorer();

var application = builder.Build();

application.MapDefaultEndpoints()
           .MapEndpoints();

application.UseHttpsRedirection()
           .UseExceptionHandler();

if (application.Environment.IsDevelopment())
{
    application.MapOpenApi();
    application.MapScalarApiReference();
}

application.Run();
