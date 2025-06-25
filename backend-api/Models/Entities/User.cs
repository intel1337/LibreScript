namespace MonApiBackend.Models.Entities;

// Objet métier Utilisateur
public class User
{
    public int Id { get; set; } // id

    public string Username { get; set; } = ""; // Nom d'utilisateur

    public string FullName { get; set; } = ""; // Nom complet de l'utilisateur

    public string Password { get; set; } = ""; // Mot de passe de l'utilisateur 

    public string Email { get; set; } = ""; // Email de l'utilisateur
    
    public bool Verified { get; set; } = false; // Indique si le compte est vérifié
    
    public string? VerificationCode { get; set; } // Code de vérification à 6 chiffres
    
    public DateTime? VerificationCodeExpiry { get; set; } // Expiration du code de vérification
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


}
