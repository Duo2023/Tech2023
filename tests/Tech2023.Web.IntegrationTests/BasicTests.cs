using Microsoft.AspNetCore.Mvc.Testing;

using Tech2023.Web.IntegrationTests.Sources;

namespace Tech2023.Web.IntegrationTests;

public class BasicTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly WebApplicationFactory<Startup> _factory;

    public BasicTests(WebApplicationFactory<Startup> factory)
    {
        _factory = factory;
    }

    [Theory]
    [ClassData(typeof(PublicRouteSource))]
    public async Task GetEndpointsReturnSuccess(string url)
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync(url);

        response.EnsureSuccessStatusCode();

        Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType!.ToString());
    }
}
