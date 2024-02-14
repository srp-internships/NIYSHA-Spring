
namespace blog_api;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(IDataContext dataContext) : base(dataContext)
    {

    }
}
