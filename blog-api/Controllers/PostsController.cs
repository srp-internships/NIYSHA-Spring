using Microsoft.AspNetCore.Mvc;

namespace blog_api;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostService service;

    public PostsController(IPostService service)
    {
        this.service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Post>>> GetAll()
    {
        return Ok(await service.GetAllAsync());
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetUserPosts(int userId)
    {
        return Ok(await service.GetUserPostsAsync(userId));
    }

    [HttpGet("paged")]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetPaging([FromQuery] int pageSize = 15, [FromQuery] int page = 1)
    {
        return Ok(await service.GetPagingAsync(pageSize, page));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePostDto dto)
    {
        await service.CreateAsync(dto);
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdatePostDto dto)
    {
        await service.UpdateAsync(dto);
        return NoContent();
    }

    [HttpDelete("{postId}")]
    public async Task<IActionResult> Delete(int postId)
    {
        await service.DeleteAsync(postId);
        return NoContent();
    }
}
