namespace MonApiBackend.;

public class Post
{
    public int Id { get; set; }       
    public string Title { get; set; } = "";     
    public string Content { get; set; } = "";   
    public string Author { get; set; } = "";    
    public DateTime CreatedAt { get; set; }   
    public DateTime? UpdatedAt { get; set; }  

 
    public void Update(string newTitle, string newContent)
    {
        Title = newTitle;
        Content = newContent;
        UpdatedAt = DateTime.UtcNow;
    }
}