using System.Text.Json.Serialization;

namespace MonApiBackend.Models.Entities
{
    public class PostVote
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        [JsonIgnore]
        public Post? Post { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
        public bool IsUpvote { get; set; } // true pour upvote, false pour downvote
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
} 