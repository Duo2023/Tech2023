using System.Diagnostics;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Tech2023.DAL;
using Tech2023.DAL.Identity;
using Tech2023.DAL.Queries;
using Tech2023.Web.Caching;
using Tech2023.Web.Shared;
using Tech2023.Web.ViewModels;

namespace Tech2023.Web.Controllers;

/// <summary>
/// Administrative controller to return views and perform admin actions
/// </summary>
[Authorize(Roles = Roles.Administrator)]
public sealed class AdminController : Controller
{
    internal readonly ILogger<AdminController> _logger;
    internal readonly IDbContextFactory<ApplicationDbContext> _factory;
    internal readonly IMemoryCache _cache;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdminController"/> class
    /// </summary>
    /// <param name="logger">The logger provided as a dependency</param>
    /// <remarks>
    /// This constructor will be discovered by the DI container and it will inject the services to the parameters
    /// </remarks>
    public AdminController(ILogger<AdminController> logger, IDbContextFactory<ApplicationDbContext> factory, IMemoryCache cache)
    {
        _logger = logger;
        _factory = factory;
        _cache = cache;
    }

    /// <summary>
    /// Returns the home admin page (panel)
    /// </summary>
    [HttpGet]
    [Route(Routes.Admin.Home)]
    public IActionResult Index()
    {
        Debug.Assert(User.Identity != null); // user has to be authenticated to reach here

        _logger.LogDebug("Admin controller accessed by: {user}", User.Identity?.Name ?? "?");

        return View();
    }

    const int DefaultPageSize = 10;

    [HttpGet]
    [Route(Routes.Admin.Subjects)]
    [ActionName(nameof(Routes.Admin.Subjects))]
    public async Task<IActionResult> EditSubjectsAsync([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
    {
        var subjects = await CachedQueries.GetSubjectViewModelsFromCacheOrFetchAsync(_factory, _cache);

        var pagedSubjects = PaginatedList<SubjectViewModel>.Create(subjects, pageNumber ?? 1, pageSize ?? DefaultPageSize);

        return View(pagedSubjects);
    }

    [HttpPost]
    [Route(Routes.Admin.DeleteSubject)]
    [ActionName(nameof(Routes.Admin.DeleteSubject))]
    public async Task<IActionResult> DeleteSubjectAsync([FromQuery] Guid id)
    {
        using var context = await _factory.CreateDbContextAsync();

        var subject = await context.Subjects.FindAsync(id);

        if (subject != null)
        {
            context.Subjects.Remove(subject);

            await context.SaveChangesAsync();
        }

        return Redirect(Request.Headers["Referer"].ToString() ?? Routes.Application.Home);
    }

    [HttpGet]
    [Route(Routes.Admin.Users)]
    [ActionName(nameof(Routes.Admin.Users))]
    public async Task<IActionResult> EditUsersAsync(
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize, 
        [FromQuery] string? searchString,
        [FromQuery] string? sortOrder
        )
    {
        using var context = await _factory.CreateDbContextAsync();

        searchString = searchString?.ToUpper();

        if (searchString != null)
        {
            pageNumber = 1;
        }

        var query = !string.IsNullOrWhiteSpace(searchString) ?
            context.Users.Where(u => u.Email != null && u.Email.Contains(searchString)) : context.Users;

        pageSize ??= DefaultPageSize; // if the page size is null use default

        var paged = await PaginatedList<ApplicationUser>.CreateAsync(query, pageNumber ?? 1, pageSize.Value);

        return View(paged);
    }
}
