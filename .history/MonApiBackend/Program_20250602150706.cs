using Microsoft.EntityFrameworkCore;
using MonApiBackend.Models.Context;

var builder = WebApplication.CreateBuilder(args);

// Ajoute la connexion PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=mysecret"));

builder.Services.AddControllers();
// Ajoute les services nécessaires pour l'API



var app = builder.Build();

app.MapControllers();
app.Run();