var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var application = builder.Build();

if (application.Environment.IsDevelopment())
{
    application.MapOpenApi();
}

application.UseHttpsRedirection();

application.Run();
