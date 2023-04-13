using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Tech2023.DAL;
using Tech2023.DAL.Models;

namespace Tech2023.Web.API.Controllers;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class PrivacyController : ControllerBase
{
    internal readonly IMemoryCache _cache;
    internal readonly IDbContextFactory<ApplicationDbContext> _factory;
    internal const string CacheSlot = "privacy_policy";

    public PrivacyController(IMemoryCache cache, IDbContextFactory<ApplicationDbContext> factory)
    {
        _cache = cache;
        _factory = factory;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        if (_cache.TryGetValue(CacheSlot, out var data))
        {
            var policy = (PrivacyPolicy?)data;

            if (policy is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(policy);
        }

        using var context = _factory.CreateDbContext();

        var privacyPolicy = await Queries.GetCurrentPrivacyPolicy(context);

        // TODO: Configure time expiry, sliding expiration etc
        _cache.Set(CacheSlot, privacyPolicy);

        return Ok(privacyPolicy);
    }
}
