namespace blog_api;

public class User : BaseEntity
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string FullName { get => $"{FirstName} {LastName}"; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string? Phone { get; set; }
    public string? CompanyName { get; set; }
    public List<Post> Posts { get; set; } = [];
}
