using Microsoft.EntityFrameworkCore;
using MonApiBackend.Models.Entities;

namespace MonApiBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostVote> PostVotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration de la table PostVotes
            modelBuilder.Entity<PostVote>()
                .HasOne(v => v.Post)
                .WithMany()
                .HasForeignKey(v => v.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PostVote>()
                .HasOne(v => v.User)
                .WithMany()
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Index unique pour empêcher les votes multiples
            modelBuilder.Entity<PostVote>()
                .HasIndex(v => new { v.PostId, v.UserId })
                .IsUnique();
        }
    }
} 