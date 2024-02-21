namespace blog_api;

public interface IPostService
{
    Task CreateAsync(CreatePostDto dto);
    Task<List<PostDto>> GetAllAsync();
    Task<List<PostDto>> GetPagingAsync(int pageSize = 15, int page = 1);
    Task<List<PostDto>> GetUserPostsAsync(int userId);
    Task UpdateAsync(UpdatePostDto dto);
    Task DeleteAsync(int postId);
}
