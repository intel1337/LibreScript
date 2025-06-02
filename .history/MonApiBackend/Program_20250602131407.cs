var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("/foo", () => "Hello from foo!");

app.MapGet("/register", () => "Hello from register!");

app.MapGet("/login", () => "Hello from login!");

/
app.MapGet("/create-post", () => "Hello from login!");



app.Run();
