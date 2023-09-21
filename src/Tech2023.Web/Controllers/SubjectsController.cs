using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Tech2023.DAL;
using Tech2023.DAL.Models;
using Tech2023.Web.API.Caching;
using Tech2023.Web.Models;

namespace Tech2023.Web.Controllers;

[Authorize]
public class SubjectsController : Controller
{
    internal readonly IDbContextFactory<ApplicationDbContext> _factory;
    internal readonly IMemoryCache _cache;
    internal readonly ILogger<SubjectsController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubjectsController"/>
    /// </summary>
    /// <param name="factory">Factory for creating database contextx</param>
    public SubjectsController(IDbContextFactory<ApplicationDbContext> factory, IMemoryCache cache, ILogger<SubjectsController> logger)
    {
        _factory = factory;
        _cache = cache;
        _logger = logger;
    }

    [Route(Routes.Subjects.Home)]
    public async Task<IActionResult> HomeAsync()
    {
        if (_cache.TryGetValue(CacheSlots.Subjects, out var data))
        {
            if (data is not SubjectListModel model)
            {
                _logger.LogError("Cache error in subjects - model returned null");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return View(model);
        }

        _logger.LogInformation("Cache missed in subjects");

        using var context = await _factory.CreateDbContextAsync();

        var subjectList = new SubjectListModel()
        {
            NceaSubjects = await GetSubjectViewModelAsync(context, CurriculumSource.Ncea),
            CambridgeSubjects = await GetSubjectViewModelAsync(context, CurriculumSource.Cambridge)
        };

        _cache.Set(CacheSlots.Subjects, subjectList);

        return View(subjectList);
    }

    internal static async Task<List<SubjectViewModel>> GetSubjectViewModelAsync(ApplicationDbContext context, CurriculumSource source)
        => await context.Subjects.Where(s => s.Source == source).Select(s => new SubjectViewModel()
        {
            Id = s.Id,
            Name = s.Name,
            Level = s.Level
        }).ToListAsync();
}
