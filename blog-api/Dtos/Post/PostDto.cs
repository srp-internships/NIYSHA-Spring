namespace blog_api;

public class PostDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Body { get; set; }
    public int UserId { get; set; }
    public string UserFullName { get; set; } = null!;
}
