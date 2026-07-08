using Microsoft.AspNetCore.Mvc;
using TSU_web_backend.Services;

namespace TSU_web_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController(ITsuNewsService tsuNewsService) : ControllerBase
{
    [HttpGet("tsu")]
    public async Task<IActionResult> GetTsuNews(CancellationToken cancellationToken)
    {
        var items = await tsuNewsService.GetLatestNewsAsync(cancellationToken);
        return Ok(items);
    }
}
