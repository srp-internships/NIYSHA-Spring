using Microsoft.EntityFrameworkCore;

namespace blog_api;

public interface IDataContext
{
    DbSet<T> Set<T>() where T : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
