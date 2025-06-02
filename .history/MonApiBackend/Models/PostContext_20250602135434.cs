using Microsoft.EntityFrameworkCore;

namespace MonApiBackend.Models;

public class PostContext : DbContext
{
    public PostContext(DbContextOptions<P> options)
        : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems { get; set; } = null!;
}