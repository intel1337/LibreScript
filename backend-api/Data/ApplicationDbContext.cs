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

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<PostVote> PostVotes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /* Équivalent SQL de la configuration PostVotes :
             * 
             * CREATE TABLE PostVotes (
             *     Id INT IDENTITY(1,1) PRIMARY KEY,
             *     PostId INT NOT NULL,
             *     UserId INT NOT NULL,
             *     VoteType VARCHAR(50) NOT NULL,
             *     CreatedAt DATETIME2 DEFAULT GETDATE()
             * );
             * 
             * ALTER TABLE PostVotes
             * ADD CONSTRAINT FK_PostVotes_Post
             * FOREIGN KEY (PostId) REFERENCES Posts(Id)
             * ON DELETE CASCADE;
             * 
             * ALTER TABLE PostVotes
             * ADD CONSTRAINT FK_PostVotes_User
             * FOREIGN KEY (UserId) REFERENCES Users(Id)
             * ON DELETE CASCADE;
             * 
             * CREATE UNIQUE INDEX IX_PostVotes_PostId_UserId
             * ON PostVotes (PostId, UserId);
             */

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