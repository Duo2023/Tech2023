using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Tech2023.Web.Shared;
using Tech2023.Web.Shared.Statistics;

namespace Tech2023.Web.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StatisticsController : ControllerBase
{
    /// <summary>
    /// Returns a ping response to indictate whether the API is up
    /// </summary>
    /// <returns>A JSON response of <see cref="PingResponse"/></returns>
    [HttpGet]
    [Route(ApiRoutes.Statistics.Ping)]
    public IActionResult Ping()
    {
        return Ok(JsonSerializer.Serialize(new PingResponse(TimeSpan.FromTicks(Environment.TickCount64), HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0")));
    }
}
