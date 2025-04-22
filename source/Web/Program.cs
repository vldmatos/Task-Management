using Web.Main;
using Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

builder.Services.AddServerSideBlazor()
                .AddCircuitOptions(options => options.DetailedErrors = true);

builder.Services.AddHttpClient<APIService>(client => client.BaseAddress = new Uri("https://api"));
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<SessionService>();
builder.Services.AddScoped<AuthenticateService>();


var application = builder.Build();

application.MapDefaultEndpoints();

if (!application.Environment.IsDevelopment())
{
    application.UseExceptionHandler("/Error", createScopeForErrors: true);
    application.UseHsts();
}

application.UseHttpsRedirection()
           .UseAntiforgery()
           .UseSession();

application.MapStaticAssets();
application.MapRazorComponents<App>()
           .AddInteractiveServerRenderMode();

application.Run();
