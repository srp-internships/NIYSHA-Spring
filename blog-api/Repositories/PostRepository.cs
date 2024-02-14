
using Microsoft.EntityFrameworkCore;

namespace blog_api;

public class PostRepository : GenericRepository<Post>, IPostRepository
{
    public PostRepository(IDataContext dataContext) : base(dataContext)
    {
    }

    public async Task<List<Post>> GetPagedList(int pageSize = 15, int page = 1, bool disableTracking = false)
    {
        IQueryable<Post> query = db.Set<Post>();

        if (disableTracking)
            query = query.AsNoTracking();

        var posts = await query.Include(x => x.User).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return posts;
    }
}
