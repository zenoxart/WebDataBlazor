using Microsoft.AspNetCore.Components;
using MudBlazor.Services;
using WebData.Components;
using WebData.Objects.PageContext.Manager;
using WebData.Objects.PageContext.Service;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

//builder.Services.AddHttpClient();

builder.Services.AddSingleton<HttpClient>();

builder.Services.AddScoped<ApiService>();

builder.Services.AddSingleton<AppBehaviorManager>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
