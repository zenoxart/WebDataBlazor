using Microsoft.AspNetCore.Components;
using MudBlazor.Services;
using WebData.Components;
using WebData.Objects.PageContext.Manager;
using WebData.Objects.PageContext.Service;

var builder = WebApplication.CreateBuilder(args);

// F�gt die Mud-Blazor UI-Komponenten hinzu
builder.Services.AddMudServices();

// F�gt Razor-Pages hinzu
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// F�gt als Singelton einen Http/Https-Client hinzu
builder.Services.AddSingleton<HttpClient>();

// F�gt als Scoped-Serivce die REST-API-Kommunikation hinzu
builder.Services.AddScoped<ApiService>();

// F�gt als Singelton die Client-Factory-Appkontext-Infrastruktur hinzu 
builder.Services.AddSingleton<AppBehaviorManager>();

// Baut die App
var app = builder.Build();

// W�hrend der Entwicklung /Error & Scrit-Transport-Security aktiviert 
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // TODO: Der Standard f�r HSTS ist 30 Tage 
    app.UseHsts();
}

// Aktiviert das Http auf Https umgeleitet wird
app.UseHttpsRedirection();

// Aktiviert die Unterst�tzung von statischen Dateien
app.UseStaticFiles();

// Verhindert Webseiten�bergriffe auf die Cookies der Anwendung
app.UseAntiforgery();

// Aktiviert Server-Rendering f�r Mudblazor in der App
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
