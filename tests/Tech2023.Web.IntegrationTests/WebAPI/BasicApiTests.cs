using Microsoft.AspNetCore.Mvc.Testing;

using Tech2023.Web.IntegrationTests.Sources;
using Tech2023.Web.Shared;

namespace Tech2023.Web.IntegrationTests.WebAPI;

public class BasicApiTests : IClassFixture<WebApplicationFactory<API.Startup>>
{
    internal readonly WebApplicationFactory<API.Startup> _factory;

    public BasicApiTests(WebApplicationFactory<API.Startup> factory)
    {
        _factory = factory;
    }

    [Theory]
    [ClassData(typeof(PublicRouteSource<ApiRoutes>))]
    public async Task GetApiEndpointsReturnsSuccessAsync(string url)
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync(url);

        response.EnsureSuccessStatusCode();
    }
}
