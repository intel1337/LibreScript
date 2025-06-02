namespace MonApiBackend.Models;

public class User
{
    public int Id { get; set; }
    [Required]
    public string Username { get; set; } = "";
    [Required]
    public string FullName { get; set; } = "";
    public string Password { get; set; } = "";
    public string Email { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


}
