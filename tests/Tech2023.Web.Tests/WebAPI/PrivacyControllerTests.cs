using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

using Tech2023.DAL.Models;
using Tech2023.Web.API.Controllers;

namespace Tech2023.Web.Tests.WebAPI;

public class PrivacyControllerTests
{
    [Fact]
    public async Task GetPrivacyPolicy_Returns_PrivacyPolicy_And_Stores_In_Cache()
    {
        // add the privacy policy to the database
        var expectedPrivacyPolicy = new PrivacyPolicy { Version = 1, Content = "Some policy content" };

        // setup a cache for it
        var cache = new MemoryCache(new MemoryCacheOptions());

        // create a logger for the rquired dependency
        var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<PrivacyController>();

        // setup the context factory as a required dependency
        var dbContextFactory = new TestDbContextFactory(DbOptions.Get());

        // initialize the controller
        var controller = new PrivacyController(cache, dbContextFactory, logger);

        // Add the expected privacy policy to the in-memory database
        await using var dbContext = dbContextFactory.CreateDbContext();

        dbContext.PrivacyPolicies.Add(expectedPrivacyPolicy);

        await dbContext.SaveChangesAsync();

        // Act
        var result = await controller.GetAsync();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        var privacyPolicy = okResult?.Value as PrivacyPolicy;

        Assert.NotNull(privacyPolicy);
        Assert.Equal(expectedPrivacyPolicy.Version, privacyPolicy!.Version);
        Assert.Equal(expectedPrivacyPolicy.Content, privacyPolicy.Content);
        Assert.Equal(expectedPrivacyPolicy.Created, privacyPolicy.Created);
        Assert.Equal(expectedPrivacyPolicy.Updated, privacyPolicy.Updated);

        // Check that the privacy policy was added to the cache
        Assert.True(cache.TryGetValue(PrivacyController.CacheSlot, out var cachedData));
        Assert.Equal(expectedPrivacyPolicy.Version, ((PrivacyPolicy)cachedData!).Version);
        Assert.Equal(expectedPrivacyPolicy.Content, ((PrivacyPolicy)cachedData).Content);
        Assert.Equal(expectedPrivacyPolicy.Created, ((PrivacyPolicy)cachedData).Created);
        Assert.Equal(expectedPrivacyPolicy.Updated, ((PrivacyPolicy)cachedData).Updated);
    }
}
