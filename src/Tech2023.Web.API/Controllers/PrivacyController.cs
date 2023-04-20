using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Tech2023.DAL;
using Tech2023.DAL.Models;
using Tech2023.Web.API.Caching;

namespace Tech2023.Web.API.Controllers;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class PrivacyController : ControllerBase
{
    internal readonly IMemoryCache _cache;
    internal readonly IDbContextFactory<ApplicationDbContext> _factory;
    internal readonly ILogger<PrivacyController> _logger;
    internal const CacheSlots CacheSlot = CacheSlots.PrivacyPolicy;

    public PrivacyController(IMemoryCache cache, IDbContextFactory<ApplicationDbContext> factory, ILogger<PrivacyController> logger)
    {
        _cache = cache;
        _factory = factory;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        if (_cache.TryGetValue(CacheSlot, out var data))
        {
            var policy = (PrivacyPolicy?)data;

            if (policy is null)
            {
                _logger.LogError("Cache in privacy policy returned a null value");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(policy);
        }

        using var context = _factory.CreateDbContext();

        var privacyPolicy = await Queries.Privacy.GetCurrentPrivacyPolicy(context);

        // TODO: Configure time expiry, sliding expiration etc
        _cache.Set(CacheSlot, privacyPolicy);

        return Ok(privacyPolicy);
    }


    /*
     * Although it may be considered 'bad practice to include an update and create in one post, we will do it anyway because it saves creating another route for pretty much the same thing
     */

    // TODO: Uncomment when authorization is done
    //[Authorize(Roles = Roles.Administrator)]
    [HttpPost]
    public async Task<IActionResult> UpdateAsync([FromQuery] string version, [FromBody] string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return BadRequest("The version is not specified");
        }

        using var context = _factory.CreateDbContext();

        PrivacyPolicy? policy;

        if (version == "create")
        {
            var newPolicy = new PrivacyPolicy()
            {
                Content = content,
                Created = DateTimeOffset.UtcNow
            };

            newPolicy.Updated = newPolicy.Created;

            context.Add(newPolicy);

            await context.SaveChangesAsync();

            _cache.Set(CacheSlot, newPolicy);

            return NoContent();
        }
        else if (version == "latest")
        {
            policy = await Queries.Privacy.GetCurrentPrivacyPolicy(context);
        }
        else if (int.TryParse(version, out int result))
        {
            policy = await Queries.Privacy.GetPrivacyByVersion(context, result);

            if (policy is null)
            {
                return BadRequest("Version is not a version contained");
            }
        }
        else
        {
            return BadRequest("The version is not a valid verison");
        }

        policy.Content = content;
        policy.Updated = DateTimeOffset.UtcNow;

        context.PrivacyPolicies.Update(policy);

        await context.SaveChangesAsync();

        _cache.Set(CacheSlot, policy);

        return Ok(policy);
    }
}
