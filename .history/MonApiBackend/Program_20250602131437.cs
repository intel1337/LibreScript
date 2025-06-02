var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


// Get endpoints
app.MapGet("/", () => "Welcome to LibreScript API, you shouldn't be here unless you hav!");
app.MapGet("/register", () => "Hello from register!");

app.MapGet("/login", () => "Hello from login!");

// Post endpoints
app.MapGet("/create-post", () => "Hello from login!");



app.Run();
