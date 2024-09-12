using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebData.Backend;

var builder = WebApplication.CreateBuilder(args);

// F�gt die API-Controller & Swagger-UI hinzu
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// F�gt die Datenbank-Infrastruktur hinzu
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Baut die App
var app = builder.Build();

// Benutzt im Dev-Mode die Swagger-UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// F�gt Websockets hinzu
app.UseWebSockets();

// Leitet Http auf Https um
app.UseHttpsRedirection();

// Aktiviert die Authorizierung der Resourcen
app.UseAuthorization();

// Mapped die API-Controller
app.MapControllers();

app.Run();
