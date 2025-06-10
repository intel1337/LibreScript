using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MonApiBackend.Models.Entities

// Objet métier d'un Commentaire
{
    public class Comment
    {
        public int Id { get; set; } // id
        public string Content { get; set; } = ""; // Contenu du commentaire

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Date de création du commentaire

        public int PostId { get; set; } // ID du post auquel le commentaire est associé
        [JsonIgnore]
        public Post? Post { get; set; }  // Post auquel le commentaire est associé (nullable et ignoré en JSON)

        public int UserId { get; set; } // ID de l'utilisateur qui a créé le commentaire
        [JsonIgnore]
        public User? User { get; set; }  // Utilisateur qui a créé le commentaire (nullable et ignoré en JSON)

        public int? ParentCommentId { get; set; } // ID du commentaire parent si c'est une réponse à un autre commentaire
        [JsonIgnore]
        public Comment? ParentComment { get; set; } // Commentaire parent si c'est une réponse (nullable et ignoré en JSON)

        [JsonIgnore]
        public ICollection<Comment> Replies { get; set; } = new List<Comment>(); // Collection de réponses à ce commentaire (ignoré en JSON)
    }
} 