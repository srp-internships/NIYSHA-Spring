namespace blog_api;

public interface IPostService
{
    Task Create(CreatePostDto dto);
    Task<List<PostDto>> GetAllAsync();
    Task<List<PostDto>> GetPaging(int pageSize = 15, int page = 1);
    Task<List<PostDto>> GetUserPostsAsync(int userId);
    Task Update(UpdatePostDto dto);
    Task Delete(int postId);
}
