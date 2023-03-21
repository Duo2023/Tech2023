using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Tech2023.Web.Shared;
using Tech2023.Web.Shared.Statistics;

namespace Tech2023.Web.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StatisticsController : ControllerBase
{
    [HttpGet]
    public IActionResult Ping()
    {
        return Ok(JsonSerializer.Serialize(new PingResponse(TimeSpan.FromTicks(Environment.TickCount64), HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0")));
    }
}
