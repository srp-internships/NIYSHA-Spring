
using AutoMapper;

namespace blog_api;

public class UserService : IUserService
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IUnitOfWork uow;
    private readonly IUserRepository repo;
    private readonly IPostRepository postRepository;
    private readonly IMapper mapper;

    public UserService(IHttpClientFactory httpClientFactory, IUnitOfWork uow, IMapper mapper)
    {
        this.httpClientFactory = httpClientFactory;
        this.uow = uow;
        repo = uow.UserRepository;
        postRepository = uow.PostRepository;
        this.mapper = mapper;
    }

    public async Task<List<UserDto>> GetAll()
    {
        var users = await repo.GetAll(disableTracking: true);

        return mapper.Map<List<UserDto>>(users);
    }

    public async Task<List<UserDto>> Search(string username)
    {
        username = username.ToUpper();

        var users = await repo.GetAll(x => x.FirstName.ToUpper().Contains(username) ||
                                           x.LastName.ToUpper().Contains(username) ||
                                           x.Username.ToUpper().Contains(username));

        return mapper.Map<List<UserDto>>(users);
    }

    // private bool Check(User user, string username)
    // {
    //     string usernameAsUpperCase = username.ToUpper();

    //     return user.FirstName.ToUpper().Contains(usernameAsUpperCase) ||
    //             user.LastName.ToUpper().Contains(usernameAsUpperCase) ||
    //             user.Username.ToUpper().Contains(usernameAsUpperCase);
    // }
}