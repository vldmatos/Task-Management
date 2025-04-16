using Client.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents();

var application = builder.Build();

if (!application.Environment.IsDevelopment())
{
    application.UseExceptionHandler("/Error", createScopeForErrors: true);
    application.UseHsts();
}

application.UseHttpsRedirection();

application.UseAntiforgery();

application.MapStaticAssets();
application.MapRazorComponents<App>();

application.Run();
