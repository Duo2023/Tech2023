using Microsoft.AspNetCore.Mvc.Testing;
using Tech2023.Web.IntegrationTests.Sources;
using System.Net;

namespace Tech2023.Web.IntegrationTests.Client;

public class BasicTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly WebApplicationFactory<Startup> _factory;

    public BasicTests(WebApplicationFactory<Startup> factory)
    {
        _factory = factory;
    }

    [Theory]
    [ClassData(typeof(PublicRouteSource<Routes>))]
    public async Task GetEndpointsReturnSuccess(string url)
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync(url);

        response.EnsureSuccessStatusCode();

        Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType!.ToString());
    }

    [Theory]
    [ClassData(typeof(AuthenticatedRouteSource<Routes>))]
    [ClassData(typeof(AdminRouteSource<Routes>))]
    public async Task GetAuthenticatedEndpointsReturnsToLogin(string url)
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync(url);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode); // if this was a web api it would return HttpStatusCode.Unauthorized but all these routes should return to login

        // assert that the response is not null and the request uri is not null
        Assert.NotNull(response.RequestMessage);
        Assert.NotNull(response.RequestMessage.RequestUri);

        // since we are not authenticated the default behaviour should be to redirect us to the login page, the uri may be uppercase or lowercase however.
        Assert.Equal(Routes.Account.Login, response.RequestMessage.RequestUri.AbsolutePath, ignoreCase: true, ignoreWhiteSpaceDifferences: true);
    }
}
