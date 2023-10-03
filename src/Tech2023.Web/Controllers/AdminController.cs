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

    /// <summary>
    /// Returns the upload page to upload resources
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route(Routes.Admin.Upload)]
    public async Task<IActionResult> Upload()
    {
        await Task.Yield();

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
    [Route(Routes.Admin.Upload)]
    public Task<IActionResult> UploadPaperAsync()
    {
        throw new NotImplementedException();
    }
}
