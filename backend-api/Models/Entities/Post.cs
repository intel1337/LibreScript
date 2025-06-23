using System.Text.Json.Serialization;

namespace MonApiBackend.Models.Entities
// Objet métier d'un Post

{
    public class Post

    {
        public int Id { get; set; } // id
        public string Title { get; set; } = ""; // Titre du post
        public string Content { get; set; } = ""; // Contenu du post
        public int UserId { get; set; } // ID de l'utilisateur qui a créé le post
        [JsonIgnore]
        public User? User { get; set; }  // Utilisateur qui a créé le post (nullable et ignoré en JSON)
        public int Upvotes { get; set; } = 0; // Nombre de upvotes
        public int Downvotes { get; set; } = 0; // Nombre de downvotes
        public string Language { get; set; } = "";   // Langue du post (0 pour français, 1 pour anglais, etc.)
        public string Status { get; set; } = "";
        public double Views { get; set; } = 0; // Nombre de vues du post
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Date de création du post
    }
}