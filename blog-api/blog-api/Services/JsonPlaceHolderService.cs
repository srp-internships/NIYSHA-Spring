
using AutoMapper;

namespace blog_api;

public class JsonPlaceHolderService : IJsonPlaceholderService
{
    private readonly IHttpClientHelper httpClientHelper;
    private readonly IMapper mapper;

    public JsonPlaceHolderService(IHttpClientHelper httpClientHelper, IMapper mapper)
    {
        this.httpClientHelper = httpClientHelper;
        this.mapper = mapper;
    }

    public async Task<List<Post>> FetchPostsAsync()
    {
        var userModels = await httpClientHelper.GetAsync<List<PostModel>>("https://jsonplaceholder.typicode.com", "posts");

        var posts = mapper.Map<List<Post>>(userModels);

        return posts;
    }

    public async Task<List<User>> FetchUsersAsync()
    {
        var userModels = await httpClientHelper.GetAsync<List<UserModel>>("https://jsonplaceholder.typicode.com", "users");

        var users = mapper.Map<List<User>>(userModels);

        return users;
    }

}
record UserModel(int Id,
                 string Name,
                 string Username,
                 string Email,
                 string Phone,
                 Address Address,
                 Company Company);

record PostModel(string Title, string Body, int UserId);

record Address(string Street, string City);

record Company(string Name);
