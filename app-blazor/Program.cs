using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using app_blazor.Shared;   // ← REQUIRED for ToastService

var builder = WebApplication.CreateBuilder(args);

// Add Razor and Blazor services
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Register HttpClient for calling app-web API
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5174"); // API base URL
});

// Inject HttpClient directly
builder.Services.AddScoped(sp =>
{
    var factory = sp.GetRequiredService<IHttpClientFactory>();
    return factory.CreateClient("ApiClient");
});

// Toasts
builder.Services.AddScoped<ToastService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
