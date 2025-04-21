using Client.Components;
using Client.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddRazorComponents();


builder.Services.AddHttpClient<APIService>(client =>
{
    client.BaseAddress = new Uri("https://api");
});

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
