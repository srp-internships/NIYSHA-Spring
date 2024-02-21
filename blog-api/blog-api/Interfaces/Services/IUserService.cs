namespace blog_api;

public interface IUserService
{
    Task<List<UserDto>> GetAllAsync();
    Task<List<UserDto>> SearchAsync(string username);
}
