using Microsoft.EntityFrameworkCore;
using MonApiBackend.Models;
using MonApiBackend.Controllers;
using MonApiBackend.Models.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.

builder.Services.AddScoped<UserController>();
builder.Services.AddScoped<PostController>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();