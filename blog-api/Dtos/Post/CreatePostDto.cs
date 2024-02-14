namespace blog_api;

public class CreatePostDto
{
    public string Title { get; set; } = null!;
    public string? Body { get; set; }
    public int UserId { get; set; }
}
