using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Tech2023.DAL;

namespace Tech2023.Web.Controllers;

[Authorize]
public class SubjectsController : Controller
{
    internal readonly IDbContextFactory<ApplicationDbContext> _factory;
    internal readonly IMemoryCache _cache;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubjectsController"/>
    /// </summary>
    /// <param name="factory">Factory for creating database contextx</param>
    public SubjectsController(IDbContextFactory<ApplicationDbContext> factory, IMemoryCache cache)
    {
        _factory = factory;
        _cache = cache;
    }

    [Route(Routes.Subjects.Home)]
    public async Task<IActionResult> HomeAsync()
    {
        await Task.CompletedTask;

        throw new NotImplementedException();
    }
}
