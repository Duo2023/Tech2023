using System.Diagnostics;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Tech2023.DAL;
using Tech2023.DAL.Models;
using Tech2023.DAL.Queries;
using Tech2023.Web.Extensions;
using Tech2023.Web.Models;
using Tech2023.Web.ViewModels;

namespace Tech2023.Web;

/// <summary>
/// The app controller, serves most of the main content of the application
/// </summary>
[Authorize]
public class AppController : Controller
{
    internal readonly ILogger<AppController> _logger;
    internal readonly IDbContextFactory<ApplicationDbContext> _context;
    internal readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppController"/>
    /// </summary>
    /// <param name="logger"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public AppController(ILogger<AppController> logger, IDbContextFactory<ApplicationDbContext> context, UserManager<ApplicationUser> userManager)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context;
        _userManager = userManager;
    }

    [Route(Routes.Application.Home)]
    public async Task<IActionResult> HomeAsync()
    {
        var savedSubjects = await Users.GetUserSavedSubjectsAsViewModelsAsync(_userManager, User);

        var model = new SubjectDashboardViewModel()
        {
            SavedSubjects = savedSubjects,
            RecentResources = new() // TODO: Add logic for seeing resources
        };

        //model.SavedSubjects.Clear();

        return View(model);
    }

    [Route(Routes.Application.PaperViewer)]
    public IActionResult PaperViewer()
    {
        return View();
    }

    [Route(Routes.Application.PaperBrowser)]
    public async Task<IActionResult> PaperBrowser(string? curriculum, string subject)
    {
        curriculum = curriculum?.ToUpper(); // TODO: Better parsing of curriculum level parsing so we don't allocate and call a nullref

        if (!Curriculum.TryParse(curriculum, out var level, out var source) || string.IsNullOrWhiteSpace(subject))
        {
            return NotFound();
        }

        using var context = await _context.CreateDbContextAsync();

        var selected = await Subjects.FindSubjectAsync(context, source, level, subject);

        if (selected is null)
        {
            return NotFound();
        }

        var browseData = new BrowsePapersViewModel
        {
            SelectedSubject = selected
        };

        return View(browseData);
    }

    [Route(Routes.Application.Assessment)]
    [ActionName(nameof(Routes.Application.Assessment))]
    public async Task<IActionResult> BrowseAssessmentAsync(string? curriculum, string subject, string standard)
    {
        curriculum = curriculum?.ToUpper(); // keep in sync with AppController browse action

        if (!Curriculum.TryParse(curriculum, out var level, out var source) || string.IsNullOrWhiteSpace(subject))
        {
            return NotFound();
        }

        using var context = await _context.CreateDbContextAsync();

        var selected = await Subjects.FindSubjectAsync(context, source, level, subject);

        if (selected == null)
        {
            return NotFound();
        }

        if (source == CurriculumSource.Ncea)
        {
            if (!int.TryParse(standard.AsSpan(), out int achievementStandard))
            {
                return NotFound();
            }

            var nceaResource = await Resources.FindNceaResourceByAchievementStandardAsync(context, achievementStandard);

            if (nceaResource == null || !selected.NceaResource.Any(r => r.Id == nceaResource.Id))
            {
                return NotFound();
            }

            return View("NceaResource", new NceaAssessmentViewModel()
            {
                Subject = (SubjectViewModel)selected,
                Resource = nceaResource
            });
        }
        else if (source == CurriculumSource.Cambridge)
        {
            if (!Cambridge.TryParseResource(standard, out var number, out var season, out var variant)) 
            {
                return NotFound();
            }

            var cambridgeResource = await Resources.FindCambridgeResourceByIdentifersAsync(context, number, season, variant);

            if (cambridgeResource == null || !selected.CambridgeResource.Any(r => r.Id == cambridgeResource.Id))
            {
                return NotFound();
            }

            return View("CambridgeResource", new CambridgeAssessmentViewModel()
            {
                Subject = (SubjectViewModel)selected,
                Resource = cambridgeResource
            });
        }

        throw new UnreachableException();
    }
}
