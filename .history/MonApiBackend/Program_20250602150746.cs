using Microsoft.EntityFrameworkCore;
using MonApiBackend.Models.Context;

// Instance de l'application ASP.NET Core
var builder = WebApplication.CreateBuilder(args);

// Ajoute la connexion PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=mysecret"));
// Ajoute les services nécessaires pour les contrôleurs
builder.Services.AddControllers();


// déclare l'application
var app = builder.Build();
// Configure l'application pour utiliser les contrôleurs
app.MapControllers();
//
app.Run();