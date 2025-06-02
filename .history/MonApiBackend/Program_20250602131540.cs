var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


// Get endpoints
app.MapGet("/", () => "Welcome to LibreScript API, you shouldn't be here unless you have the bearer ^^!");
app.MapGet("/register", () => "Hello from register!");

app.MapGet("/login", () => "Hello from login!");

// Post endpoints
app.MapPost("/create-post", () => "Hello from login!");

app.MapGet("/get-posts", () => "Hello from get posts!");

app.MapGet("/get-post/{id}", (int id) => $"Hello from get post with id {id}!");
app.MapPost("/create-comment", () => "Hello from create comment!");







app.Run();
