namespace MonApiBackend.Models.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; } // Assuming posts are linked to users
        public int UserId { get; set; }
public User User { get; set; }
        public User User { get; set; } // Navigation property to the User model
    }
}