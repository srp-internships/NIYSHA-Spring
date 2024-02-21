using System.Linq.Expressions;

namespace blog_api;

public interface IRepository<T> where T : BaseEntity
{
    void Create(T entity);
    void Create(IEnumerable<T> entities);
    Task<T?> GetByIdAsync(int id, string includeProperties = "", bool disableTracking = false);
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression = null!, string includeProperties = "", bool disableTracking = false);
    void Update(T entity);
    void Delete(T entity);
}
