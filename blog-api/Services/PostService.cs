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

    public async Task Create(CreatePostDto dto)
    {
        var user = uow.UserRepository.GetById(dto.UserId);
        if (user is null)
            throw new Exception("User with provided Id was not found.");

        repo.Create(mapper.Map<Post>(dto));
        await uow.SaveChangesAsync();
    }

    public async Task Delete(int postId)
    {
        var post = await repo.GetById(postId);

        if (post is null)
            throw new Exception("Post with provided Id was not found.");

        repo.Delete(post);

        await uow.SaveChangesAsync();
    }

    public async Task<List<PostDto>> GetAllAsync()
    {
        var posts = await repo.GetAll(includeProperties: "User", disableTracking: true);

        return mapper.Map<List<PostDto>>(posts);
    }

    public async Task<List<PostDto>> GetPaging(int pageSize = 15, int page = 1)
    {
        var posts = await repo.GetPagedList(pageSize, page, true);

        return mapper.Map<List<PostDto>>(posts);
    }

    public async Task<List<PostDto>> GetUserPostsAsync(int userId)
    {
        var posts = await repo.GetAll(x => x.UserId == userId, "User", disableTracking: true);

        return mapper.Map<List<PostDto>>(posts);
    }

    public async Task Update(UpdatePostDto dto)
    {
        var post = await repo.GetById(dto.Id);

        if (post is null)
            throw new Exception("Post with provided Id was not found.");

        mapper.Map(dto, post);

        await uow.SaveChangesAsync();
    }
}