using System.Net;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Tech2023.Web.API.Controllers;
using Tech2023.Web.Shared.Statistics;

using Xunit;

namespace Tech2023.Web.Tests.WebAPI;

public class StatisticsControllerTests
{
    private readonly StatisticsController _controller;

    public StatisticsControllerTests()
    {
        _controller = new StatisticsController()
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }

    // Update logic here if the code returns 

    [Fact]
    public void Ping_ReturnsOkResult_WithPingResponse()
    {
        var result = _controller.Ping();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResponse = Assert.IsType<PingResponse>(okResult.Value);

        Assert.True(actualResponse.Runtime > TimeSpan.Zero);
        Assert.NotNull(actualResponse.Ip);

        Assert.True(IPAddress.TryParse(actualResponse.Ip, out _));
    }
}
