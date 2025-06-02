using Microsoft.EntityFrameworkCore;

namespace MonApiBackend.Models;

public class PostContet : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems { get; set; } = null!;
}