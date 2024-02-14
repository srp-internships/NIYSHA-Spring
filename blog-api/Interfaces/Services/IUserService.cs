namespace blog_api;

public interface IUserService
{
    Task<List<UserDto>> GetAll();
    Task<List<UserDto>> Search(string username);
}
