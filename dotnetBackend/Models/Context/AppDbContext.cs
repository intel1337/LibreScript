namespace MonApiBackend.Models.Context;
using MonApiBackend.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

public class AppDbContext : DbContext 
{
    public AppDbContext(DbContextOptions<AppDbContext> options) // Constructeur de la classe qui prend en paramètre les options de DbContext
        : base(options) 
    {
    }
    public DbSet<User> Users { get; set; } = null!; // DbSet pour les utilisateurs, ne peut être null
    public DbSet<Post> Posts { get; set; } = null!; // DbSet pour les posts, ne peut être null
    public DbSet<Category> Categories { get; set; } = null!; // DbSet pour les catégories, ne peut être null
    public DbSet<Comment> Comments { get; set; } = null!;   // DbSet pour les commentaires, ne peut être null
    public DbSet<PostVote> PostVotes { get; set; } = null!; // DbSet pour les votes sur les posts, ne peut être null
}