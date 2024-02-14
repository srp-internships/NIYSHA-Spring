using AutoMapper;

namespace blog_api;

public class PostService : IPostService
{
    private readonly IUnitOfWork uow;
    private readonly IPostRepository repo;
    private readonly IMapper mapper;

    public PostService(IUnitOfWork uow, IMapper mapper)
    {
        this.uow = uow;
        repo = uow.PostRepository;
        this.mapper = mapper;
    }

    public async Task CreateAsync(CreatePostDto dto)
    {
        var user = await uow.UserRepository.GetByIdAsync(dto.UserId);
        if (user is null)
            throw new Exception("User with provided Id was not found.");

        repo.Create(mapper.Map<Post>(dto));
        await uow.SaveChangesAsync();
    }

    public async Task DeleteAsync(int postId)
    {
        var post = await repo.GetByIdAsync(postId);

        if (post is null)
            throw new Exception("Post with provided Id was not found.");

        repo.Delete(post);

        await uow.SaveChangesAsync();
    }

    public async Task<List<PostDto>> GetAllAsync()
    {
        var posts = await repo.GetAllAsync(includeProperties: "User", disableTracking: true);

        return mapper.Map<List<PostDto>>(posts);
    }

    public async Task<List<PostDto>> GetPagingAsync(int pageSize = 15, int page = 1)
    {
        var posts = await repo.GetPagedList(pageSize, page, true);

        return mapper.Map<List<PostDto>>(posts);
    }

    public async Task<List<PostDto>> GetUserPostsAsync(int userId)
    {
        var posts = await repo.GetAllAsync(x => x.UserId == userId, "User", disableTracking: true);

        return mapper.Map<List<PostDto>>(posts);
    }

    public async Task UpdateAsync(UpdatePostDto dto)
    {
        var post = await repo.GetByIdAsync(dto.Id);

        if (post is null)
            throw new Exception("Post with provided Id was not found.");

        mapper.Map(dto, post);

        await uow.SaveChangesAsync();
    }
}