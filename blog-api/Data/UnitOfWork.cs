
namespace blog_api;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDataContext db;

    public UnitOfWork(IDataContext dataContext)
    {
        db = dataContext;
    }

    private IUserRepository? userRepository;
    private IPostRepository? postRepository;

    public IUserRepository UserRepository
    {
        get
        {
            return userRepository ??= new UserRepository(db);
        }
    }

    public IPostRepository PostRepository
    {
        get
        {
            return postRepository ??= new PostRepository(db);
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await db.SaveChangesAsync(cancellationToken);
    }
}
