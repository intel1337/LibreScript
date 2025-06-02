namespace MonApiBackend.Models.Entities;

using MonApiBackend.Models.Entities;

public class Reply
{
    public int Id { get; set; }
    public User User { get; set; }
    public string Title{ get; set; }

}
