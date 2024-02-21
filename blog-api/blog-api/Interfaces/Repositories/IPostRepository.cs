namespace blog_api;

public interface IPostRepository : IRepository<Post>
{
    Task<List<Post>> GetPagedList(int pageSize = 15, int page = 1, bool disableTracking = false);
}
