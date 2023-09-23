using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Tech2023.DAL;
using Tech2023.Web.API.Caching;
using Tech2023.Web.Models;

namespace Tech2023.Web.Controllers;

/// <summary>
/// Provides methods to manipulate the users subjects collections
/// </summary>
[Authorize]
public class SubjectsController : Controller
{
    internal readonly IDbContextFactory<ApplicationDbContext> _factory;
    internal readonly IMemoryCache _cache;
    internal readonly ILogger<SubjectsController> _logger;
    internal readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubjectsController"/> class
    /// </summary>
    public SubjectsController(IDbContextFactory<ApplicationDbContext> factory, IMemoryCache cache, ILogger<SubjectsController> logger, UserManager<ApplicationUser> userManager)
    {
        _factory = factory;
        _cache = cache;
        _logger = logger;
        _userManager = userManager;
    }

    [Route(Routes.Subjects.Home)]
    public async Task<IActionResult> HomeAsync()
    {
        List<SubjectViewModel> savedSubjects = await GetSubjectsAsync(User); // the users saved subjects
        List<SubjectViewModel> subjects; // the subjects they can select

        using var context = await _factory.CreateDbContextAsync();

        if (_cache.TryGetValue(CacheSlots.Subjects, out var data)) // fast path
        {
            if (data is not List<SubjectViewModel> list)
            {
                _logger.LogError("Cache error in subjects - model returned null");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            subjects = list;
        }
        else // slow path
        {
            _logger.LogInformation("Cache missed in subjects");
            subjects = await GetSubjectViewModelAsync(context);
            _cache.Set(CacheSlots.Subjects, subjects);
        }

        var subjectsList = new SubjectListModel()
        {
            Subjects = subjects,
            ExistingSubjects = savedSubjects
        };

        return View(subjectsList);
    }

    [HttpPost]
    [Route(Routes.Subjects.Edit)]
    public async Task<IActionResult> EditAsync()
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

#nullable disable
    internal async Task<List<SubjectViewModel>> GetSubjectsAsync(ClaimsPrincipal principal)
    {
        string userName = _userManager.NormalizeEmail(principal.Identity.Name);

        return await _userManager.Users.Where(s => s.NormalizedUserName == userName)
            .Include(s => s.SavedSubjects)
            .Select(s => s.SavedSubjects)
            .SelectMany(list => list)
            .Select(s => (SubjectViewModel)s)
            .ToListAsync();
    }

    // This internal method transforms subjects into view models so we don't pass useless state
    internal static async Task<List<SubjectViewModel>> GetSubjectViewModelAsync(ApplicationDbContext context)
        => await context.Subjects.Select(s => (SubjectViewModel)s).ToListAsync();
}
