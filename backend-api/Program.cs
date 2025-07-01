using Microsoft.EntityFrameworkCore;
using MonApiBackend.Models.Context;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args); // Instancie le builder
const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins"; // Cors _ pour instance privée de la classe

// Instancie le cors dans les services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configure JWT Authentication
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Value;
if (string.IsNullOrEmpty(jwtKey))
{
    throw new InvalidOperationException("JWT Key not found in configuration");
}

var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// Configuration HttpClient pour l'API IA
builder.Services.AddHttpClient();

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

// Ajout des services personnalisés
builder.Services.AddScoped<MonApiBackend.Lib.IVerificationService, MonApiBackend.Lib.VerificationService>();

// Ajout des services pour les contrôleurs
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
// Rate limiter
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,
                QueueLimit = 10,
                Window = TimeSpan.FromMinutes(1)
            }));
});
// Déclaration de l'application
var app = builder.Build();

// On configure le CORS en premier pour gérer les requêtes preflight
app.UseCors("AllowAll");

// On configure le routage
app.UseRouting(); 

// On configure l'authentification (ordre important!)
app.UseAuthentication();
app.UseAuthorization();

// On configure les contrôleurs
app.MapControllers();
// On configure le rate limiter
app.UseRateLimiter();
// boostrap de l'application
app.Run();