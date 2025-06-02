namespace MonApiBackend.Models.Context;
using MonApiBackend.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

// Contexte de la base de données pour l'application pour le MVC

public class AppDbContext : DbContext // Déclaration de la class qui hérite de DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) // Constructeur de la classe qui prend en paramètre les options de DbContext
        : base(options) 
    {
    }
    public DbSet<User> Users { get; set; } = null!; // DbSet pour les utilisateurs, ne peut être null
    public DbSet<Post> Posts { get; set; } = null!; // DbSet pour les posts, ne peut être null
    public DbSet<Category> Categories { get; set; } = null!; // DbSet pour les catégories, ne peut être null
    public DbSet<Comment> Comments { get; set; } = null!;   // DbSet pour les commentaires, ne peut être null


}