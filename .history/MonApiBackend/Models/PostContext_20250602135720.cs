using Microsoft.EntityFrameworkCore;

namespace MonApiBackend.Models;

public class PostContext : DbContext
{
    public PostContext(DbContextOptions<PostContext> options)
        : base(options)
    {
        <Post
    }

    public DbSet<PostItem> PostItems { get; set; } = null!;
}