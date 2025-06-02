namespace MonApiBackend.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public void Update(string newName, string newDescription)
    {
        Name = newName;
        Description = newDescription;
        UpdatedAt = DateTime.UtcNow;
    }

}
