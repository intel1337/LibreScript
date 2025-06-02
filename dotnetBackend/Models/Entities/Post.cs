namespace MonApiBackend.Models.Entities
// Objet métier d'un Post

{
    public class Post
    {
        public int Id { get; set; } // id
        public string Title { get; set; } = ""; // Titre du post
        public string Content { get; set; }= ""; // Contenu du post
        public int UserId { get; set; } // ID de l'utilisateur qui a créé le post
        public required User User { get; set; }  // Utilisateur qui a créé le post
    }
}