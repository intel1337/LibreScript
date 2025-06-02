using Microsoft.EntityFrameworkCore;
using MonApiBackend.Models.Context;

// Instance
var builder = WebApplication.CreateBuilder(args);

// Ajoute la connexion PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=mysecret"));

builder.Services.AddControllers();



var app = builder.Build();

app.MapControllers();
app.Run();