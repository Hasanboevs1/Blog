

namespace PostService.Entities;
public class Post
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
     public long AuthorId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
