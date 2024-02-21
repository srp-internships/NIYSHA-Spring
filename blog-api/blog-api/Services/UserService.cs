
using AutoMapper;

namespace blog_api;

public class UserService : IUserService
{
    private readonly IUnitOfWork uow;
    private readonly IUserRepository repo;
    private readonly IPostRepository postRepository;
    private readonly IMapper mapper;

    public UserService(IUnitOfWork uow, IMapper mapper)
    {
        this.uow = uow;
        repo = uow.UserRepository;
        postRepository = uow.PostRepository;
        this.mapper = mapper;
    }

    public async Task<List<UserDto>> GetAllAsync()
    {
        var users = await repo.GetAllAsync(disableTracking: true);

        return mapper.Map<List<UserDto>>(users);
    }

    public async Task<List<UserDto>> SearchAsync(string username)
    {
        username = username.ToUpper();

        var users = await repo.GetAllAsync(x => x.FirstName.ToUpper().Contains(username) ||
                                           x.LastName.ToUpper().Contains(username) ||
                                           x.Username.ToUpper().Contains(username), disableTracking: true);

        return mapper.Map<List<UserDto>>(users);
    }
}