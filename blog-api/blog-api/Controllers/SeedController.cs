using Microsoft.AspNetCore.Mvc;

namespace blog_api;

[ApiController]
[Route("[controller]")]
public class SeedController : ControllerBase
{
    private readonly ISeedService service;

    public SeedController(ISeedService service)
    {
        this.service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Seed()
    {
        await service.SeedDatabaseAsync();
        return Ok();
    }
}
