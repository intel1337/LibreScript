namespace MonApiBackend;

public class Post
{
    public int Id { get; set; }       
    public string Title { get; set; } = "";     // Titre du post
    public string Content { get; set; } = "";   // Contenu du message
    public string Author { get; set; } = "";    // Nom ou identifiant de l'auteur
    public DateTime CreatedAt { get; set; }     // Date de création
    public DateTime? UpdatedAt { get; set; }    // Date de modification (optionnelle)

    // Méthode utile pour mettre à jour le contenu
    public void Update(string newTitle, string newContent)
    {
        Title = newTitle;
        Content = newContent;
        UpdatedAt = DateTime.UtcNow;
    }
}