namespace MonApiBackend.Models;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = "";

    public string FullName { get; set; } = "";
    [Required]
    public string Password { get; set; } = "";
    [Required]
    public string Email { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


}
