using Microsoft.EntityFrameworkCore;

namespace MonApiBackend.Models.Context;

public class PostContext : DbContext
{
    public PostContext(DbContextOptions<PostContext> options)
        : base(options)
    {

    }

    public DbSet<PostItem> PostItems { get; set; } = null!;
}