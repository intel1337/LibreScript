namespace MonApiBackend;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = "";

    public 
    public string Password { get; set; } = "";
    public string Email { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

}
