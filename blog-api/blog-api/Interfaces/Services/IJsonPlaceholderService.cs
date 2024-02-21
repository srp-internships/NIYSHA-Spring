namespace blog_api;

public interface IJsonPlaceholderService
{
    Task<List<User>> FetchUsersAsync();
    Task<List<Post>> FetchPostsAsync();
}
