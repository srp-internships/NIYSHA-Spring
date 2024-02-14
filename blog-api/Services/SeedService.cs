
using System.Reflection;
using AutoMapper;
using Newtonsoft.Json;

namespace blog_api;

public class SeedService : ISeedService
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IUnitOfWork uow;
    private readonly IUserRepository userRepository;
    private readonly IPostRepository postRepository;

    public SeedService(IHttpClientFactory httpClientFactory, IUnitOfWork uow, IMapper mapper)
    {
        this.httpClientFactory = httpClientFactory;
        this.uow = uow;
        userRepository = uow.UserRepository;
        postRepository = uow.PostRepository;
    }

    public async Task SeedDatabase()
    {
        var usersFromDb = await userRepository.GetAllAsync();
        var postsFromDb = await postRepository.GetAllAsync();

        if (usersFromDb.Count == 0)
        {
            var response = await MakeRequest(HttpMethod.Get, "https://jsonplaceholder.typicode.com/users");

            if (response.IsSuccessStatusCode)
            {
                var users = await DeserializeJsonToUsers(response);

                userRepository.Create(users);
                await uow.SaveChangesAsync();
            }
        }

        if (postsFromDb.Count == 0)
        {
            var response = await MakeRequest(HttpMethod.Get, "https://jsonplaceholder.typicode.com/posts");

            if (response.IsSuccessStatusCode)
            {
                var posts = await DeserializeJsonToPosts(response);

                postRepository.Create(posts);
            }
        }

        await uow.SaveChangesAsync();
    }

    private async Task<List<User>> DeserializeJsonToUsers(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();

        var userModel = new[]
        {
                new
                {
                    Id = 0,
                    Name = "",
                    Username = "",
                    Email = "",
                    Address = new
                    {
                        Street = "",
                        City = ""
                    },
                    Phone = "",
                    Company = new
                    {
                        Name = ""
                    }
                }
            };

        var userModels = JsonConvert.DeserializeAnonymousType(json, userModel);

        if (userModels is null)
            throw new Exception($"Something went wrong. Please check {this.GetType().Name}.{MethodBase.GetCurrentMethod()!.Name} method.");

        var users = userModels.Select(u =>
        {
            var nameSplit = u.Name.Split(" ");

            var user = new User()
            {
                Address = $"{u.Address.City} {u.Address.Street}",
                CompanyName = u.Company.Name,
                Email = u.Email,
                FirstName = nameSplit[0].Contains('.') ? (nameSplit.Length == 3 ? nameSplit[1] : "") : nameSplit[0],
                LastName = nameSplit[0].Contains('.') ? (nameSplit.Length == 3 ? nameSplit[2] : "") : nameSplit[1],
                Phone = u.Phone,
                Username = u.Username
            };

            return user;
        }).ToList();

        return users;
    }

    private async Task<List<Post>> DeserializeJsonToPosts(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();

        var postModel = new[]
        {
                new
                {
                    UserId = 0,
                    Id = 1,
                    Title = "",
                    Body = ""
                }
            };

        var postModels = JsonConvert.DeserializeAnonymousType(json, postModel);

        if (postModels is null)
            throw new Exception($"Something went wrong. Please check {GetType().Name}.{MethodBase.GetCurrentMethod()!.Name} method.");

        var posts = postModels.Select(p =>
        {
            var post = new Post()
            {
                UserId = p.UserId,
                Body = p.Body,
                Title = p.Title
            };

            return post;
        }).ToList();

        return posts;
    }

    private async Task<HttpResponseMessage> MakeRequest(HttpMethod method, string uri)
    {
        var httpRequestMessage = new HttpRequestMessage(method, uri);
        var httpClient = httpClientFactory.CreateClient();
        var response = await httpClient.SendAsync(httpRequestMessage);
        return response;
    }
}
