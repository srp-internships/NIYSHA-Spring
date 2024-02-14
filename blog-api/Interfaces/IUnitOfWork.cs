namespace blog_api;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IPostRepository PostRepository { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
