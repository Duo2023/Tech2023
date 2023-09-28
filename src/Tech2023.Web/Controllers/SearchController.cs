using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Tech2023.DAL;
using Tech2023.Web.ViewModels;

namespace Tech2023.Web.Controllers;

[Authorize]
public class SearchController : Controller
{
    internal readonly IMemoryCache _cache;
    internal readonly IDbContextFactory<ApplicationDbContext> _factory;

    public SearchController(IMemoryCache cache, IDbContextFactory<ApplicationDbContext> factory)
    {
        _cache = cache;
        _factory = factory;
    }

    [HttpGet]
    [ActionName(nameof(Routes.Search))]             
    [Route(Routes.Search)]
    public async Task<IActionResult> SearchAsync([FromQuery] string query)
    {
        List<SearchResult> results = new();

        if (string.IsNullOrEmpty(query))
        {
            return View(results);
        }

        query = query.ToUpper();

        using var context = await _factory.CreateDbContextAsync();

        if (query.Contains("PRIVACY"))
        {
            results.Add(new SearchResult()
            {
                Name = "Privacy Policy Page",
                Url = Routes.Privacy
            });
        }

        return View(results);
    }
}
