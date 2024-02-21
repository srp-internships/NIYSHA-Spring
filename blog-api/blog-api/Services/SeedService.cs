
using AutoMapper;

namespace blog_api;

public class SeedService : ISeedService
{
    private readonly IJsonPlaceholderService jsonPlaceholderService;
    private readonly IUnitOfWork uow;
    private readonly IUserRepository userRepository;
    private readonly IPostRepository postRepository;

    public SeedService(IJsonPlaceholderService jsonPlaceholderService, IUnitOfWork uow, IMapper mapper)
    {
        this.jsonPlaceholderService = jsonPlaceholderService;
        this.uow = uow;
        userRepository = uow.UserRepository;
        postRepository = uow.PostRepository;
    }

    public async Task SeedDatabaseAsync()
    {
        var usersFromDb = await userRepository.GetAllAsync();

        if (usersFromDb.Count == 0)
        {
            var users = await jsonPlaceholderService.FetchUsersAsync();
            var posts = await jsonPlaceholderService.FetchPostsAsync();

            users.ForEach(u =>
            {
                u.Posts = posts.Where(p => p.UserId == u.Id).ToList();
                u.Id = 0;
            });

            userRepository.Create(users);
            await uow.SaveChangesAsync();
        }
    }
}
