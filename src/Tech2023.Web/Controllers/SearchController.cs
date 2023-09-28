using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Tech2023.DAL;
using Tech2023.Web.API.Caching;
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
        SearchResults search = new()
        {
            Query = query,
            Results = new()
        };

        if (string.IsNullOrEmpty(query))
        {
            return View(search);
        }

        query = query.ToUpper();

        using var context = await _factory.CreateDbContextAsync();

        if (query.Contains("PRIVACY"))
        {
            search.Results.Add(SearchResult.Create("Privacy Policy", Routes.Privacy));
        }

        if (_cache.TryGetValue(CacheSlots.Subjects, out var data))
        {
            if (data is List<SubjectViewModel> subjects)
            {
                foreach (var item in subjects.Where(s => s.Name.Contains(query)))
                {
                    var str = Curriculum.ToString(item.Level, item.Source);

                    search.Results.Add(SearchResult.Create($"{item.Name} - {str}", $"/browse/{str}/{item.Name}"));
                }
            }
        }

        return View(search);
    }
}
