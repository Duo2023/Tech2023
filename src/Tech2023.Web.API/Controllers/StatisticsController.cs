using Microsoft.AspNetCore.Mvc;
using Tech2023.Web.Shared;

namespace Tech2023.Web.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StatisticsController : ControllerBase
{
    [Route(ApiRoutes.Statistics.Ping)]
    [HttpGet]
    public async Task<IActionResult> PingAsync()
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }
}
