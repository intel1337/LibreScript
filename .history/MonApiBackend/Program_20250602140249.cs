using Microsoft.EntityFrameworkCore;
using MonApiBackend.Models;
using MonApiBackend.Controllers;
using MonApiBackend.Models.Context;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<PostContext>(opt =>
    opt.UseInMemoryDatabase("TestDatabase"));
// Register the UserController as a service
builder.Services.AddScoped<UserController>();
// Register the PostController as a service
builder.Services.AddScoped<PostController>();
// Register the CommentController as a service

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.MapControllers();


// Get endpoints
app.MapGet("/", () => "Welcome to LibreScript API, you shouldn't be here unless you have the bearer ^^!");

// User endpoints
app.MapGet("/get-user/{id}", (int id) => $"Hello from get user with id {id}!");
app.MapGet("/get-user/{username}", (string username) => $"Hello from get user with username {username}!");
app.MapGet("/get-users", () => "Hello from get users!");

app.MapPost("/register", () => "Hello from register!");
app.MapPost("/login", () => "Hello from login!");

// Post endpoints
app.MapPost("/create-post", () => "Hello from login!");

app.MapGet("/get-posts", () => "Hello from get posts!");

app.MapGet("/get-post/{id}", (int id) => $"Hello from get post with id {id}!");
app.MapGet("/get-post/{user}", (string user) => $"Hello from get post with id {user}!");


// Comment endpoints
app.MapPost("/create-comment", () => "Hello from create comment!");
app.MapGet("/get-comment/{id}", (int id) => $"Hello from get comment with id {id}!");
app.MapGet("/get-comments/{postId}", (int postId) => $"Hello from get comments with post id {postId}!");









app.Run();
