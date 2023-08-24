using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Tech2023.DAL;
using Tech2023.DAL.Models;
using Tech2023.Web.API.Caching;
using Tech2023.Web.Models;
using Tech2023.Web.Shared;

namespace Tech2023.Web.Controllers;

public class HomeController : Controller
{
    internal readonly ILogger<HomeController> _logger;
    internal readonly IMemoryCache _cache;
    internal readonly IDbContextFactory<ApplicationDbContext> _factory;

    public HomeController(ILogger<HomeController> logger, IMemoryCache cache, IDbContextFactory<ApplicationDbContext> factory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _cache = cache;
        _factory = factory;
    }

    [Route(Routes.Home)]
    public IActionResult Index()
    {
        return View();
    }

    [Route("/Home/HandleError/{code:int}")]
    public IActionResult HandleError(int code)
    {
        return code switch
        {
            StatusCodes.Status404NotFound => View("~/Views/Shared/404.cshtml"),
            _ => View("~/Views/Shared/ErrorCode.cshtml", code),
        };
    }

    [Route(Routes.Privacy)]
    public async Task<IActionResult> PrivacyAsync()
    {
        if (_cache.TryGetValue(CacheSlots.PrivacyPolicy, out var data))
        {
            if (data is not PrivacyPolicy policy || policy is null)
            {
                _logger.LogError("Cache in privacy policy returned null");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return View(policy);
        }

        using var context = _factory.CreateDbContext();

        var privacyPolicy = await Queries.Privacy.GetCurrentPrivacyPolicy(context);

        // TODO: Configure time expiry, sliding expiration etc
        _cache.Set(CacheSlots.PrivacyPolicy, privacyPolicy);

        return View(privacyPolicy);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
