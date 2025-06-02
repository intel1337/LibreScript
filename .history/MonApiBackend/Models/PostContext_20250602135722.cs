using Microsoft.EntityFrameworkCore;

namespace MonApiBackend.Models;

public class PostContext : DbContext
{
    public PostContext(DbContextOptions<PostContext> options)
        : base(options)
    {
        <PostItems = Set<PostItem>();
    }

    public DbSet<PostItem> PostItems { get; set; } = null!;
}