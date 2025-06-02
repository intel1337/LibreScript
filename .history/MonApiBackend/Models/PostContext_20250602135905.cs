using Microsoft.EntityFrameworkCore;

namespace MonApiBackend.Models.cont;

public class PostContext : DbContext
{
    public PostContext(DbContextOptions<PostContext> options)
        : base(options)
    {

    }

    public DbSet<PostItem> PostItems { get; set; } = null!;
}