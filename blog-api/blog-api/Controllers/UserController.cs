using Microsoft.AspNetCore.Mvc;

namespace blog_api;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService service;

    public UserController(IUserService service)
    {
        this.service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        return Ok(await service.GetAllAsync());
    }

    [HttpGet("search/{username}")]
    public async Task<ActionResult<List<UserDto>>> Search(string username)
    {
        return Ok(await service.SearchAsync(username));
    }
}