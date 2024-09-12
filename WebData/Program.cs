using Microsoft.AspNetCore.Components;
using MudBlazor.Services;
using WebData.Components;
using WebData.Objects.PageContext.Manager;
using WebData.Objects.PageContext.Service;

var builder = WebApplication.CreateBuilder(args);

// Fügt die Mud-Blazor UI-Komponenten hinzu
builder.Services.AddMudServices();

// Fügt Razor-Pages hinzu
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Fügt als Singelton einen Http/Https-Client hinzu
builder.Services.AddSingleton<HttpClient>();

// Fügt als Scoped-Serivce die REST-API-Kommunikation hinzu
builder.Services.AddScoped<ApiService>();

// Fügt als Singelton die Client-Factory-Appkontext-Infrastruktur hinzu 
builder.Services.AddSingleton<AppBehaviorManager>();

// Baut die App
var app = builder.Build();

// Während der Entwicklung /Error & Scrit-Transport-Security aktiviert 
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // TODO: Der Standard für HSTS ist 30 Tage 
    app.UseHsts();
}

// Aktiviert das Http auf Https umgeleitet wird
app.UseHttpsRedirection();

// Aktiviert die Unterstützung von statischen Dateien
app.UseStaticFiles();

// Verhindert Webseitenübergriffe auf die Cookies der Anwendung
app.UseAntiforgery();

// Aktiviert Server-Rendering für Mudblazor in der App
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
