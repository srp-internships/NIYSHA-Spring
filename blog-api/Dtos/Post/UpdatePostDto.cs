namespace blog_api;

public class UpdatePostDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
}
