
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace blog_api;

public class GenericRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly IDataContext db;

    public GenericRepository(IDataContext dataContext)
    {
        db = dataContext;
    }

    public void Create(T entity)
    {
        db.Set<T>().Add(entity);
    }

    public void Create(IEnumerable<T> entities)
    {
        db.Set<T>().AddRange(entities);
    }

    public void Delete(T entity)
    {
        db.Set<T>().Remove(entity);
    }

    public async Task<List<T>> GetAll(Expression<Func<T, bool>> expression = null!, string includeProperties = "", bool disableTracking = false)
    {
        IQueryable<T> query = db.Set<T>();

        if (expression is not null)
            query = query.Where(expression);

        if (disableTracking)
            query = query.AsNoTracking();

        foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        return await query.ToListAsync();
    }

    public async Task<T?> GetById(int id, string includeProperties = "", bool disableTracking = false)
    {
        IQueryable<T> query = db.Set<T>();


        if (disableTracking)
            query = query.AsNoTracking();

        foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        return await query.FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Update(T entity)
    {
        db.Set<T>().Update(entity);
    }
}
