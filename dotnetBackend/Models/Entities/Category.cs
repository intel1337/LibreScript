namespace MonApiBackend.Models.Entities;
// objet métier d'une Catégorie

public class Category
{
    public int Id { get; set; } // id
    public string Name { get; set; } = ""; // Nom de la catégorie
    public string Description { get; set; } = ""; // Description de la catégorie
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Date de création de la catégorie




}
