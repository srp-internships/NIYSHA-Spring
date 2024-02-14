namespace blog_api;

public class Post : BaseEntity
{
    public string Title { get; set; } = null!;
    public string? Body { get; set; }
    public User User { get; set; } = null!;
    public int UserId { get; set; }
}
