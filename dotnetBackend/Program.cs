using Microsoft.EntityFrameworkCore;
using MonApiBackend.Models.Context;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args); // Instancie le builder
const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins"; // Cors _ pour instance privée de la classe

// Instancie le cors dans les services
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
            policy  =>
            {
                policy.WithOrigins("http://localhost:5173", 
                                    "http://127.0.0.1:5173")  
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
});

// déclaration de la connection string, depuis les appsettings.json // Peut être null si non existente
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    // Si la connection string n'est pas définie dans appsettings.json, on utilise une valeur par défaut
    connectionString = "Host=localhost;Port=5432;Database=your_db;Username=testuser;Password=testpass";

}
// Ajout du DbContext avec la connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
);
// Ajout des services pour les contrôleurs
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });


// Déclaration de l'application
var app = builder.Build();
// On configure le routage
app.UseRouting(); 
// On configure le CORS
app.UseCors(MyAllowSpecificOrigins); 
// On configure l'authentification
app.UseAuthorization();

// On configure les contrôleurs
app.MapControllers();

// boostrap de l'application
app.Run();