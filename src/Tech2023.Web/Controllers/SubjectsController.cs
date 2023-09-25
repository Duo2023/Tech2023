using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Tech2023.DAL;
using Tech2023.DAL.Queries;
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
        List<SubjectViewModel> savedSubjects = await Users.GetUserSavedSubjectsAsViewModelsAsync(_userManager, User);
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
            subjects = await Subjects.GetAllSubjectViewModelsAsync(context);
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
}
