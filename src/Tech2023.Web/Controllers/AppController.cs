using System.Diagnostics;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Tech2023.DAL;
using Tech2023.DAL.Identity;
using Tech2023.DAL.Models;
using Tech2023.DAL.Queries;
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

    [Route(Routes.Application.PaperBrowser)]
    public async Task<IActionResult> PaperBrowser(string? curriculum, string subject)
    {
        curriculum = curriculum?.ToUpper(); // TODO: Better parsing of curriculum level parsing so we don't allocate and call a nullref

        // parse what curriculum source and level if any. check if subject is null
        if (!Curriculum.TryParse(curriculum, out var level, out var source) || string.IsNullOrWhiteSpace(subject))
        {
            return NotFound();
        }

        // create context
        using var context = await _context.CreateDbContextAsync();

        // find the subject
        var selected = await Subjects.FindSubjectAsync(context, source, level, subject);

        // return not found if subject doesn't exist
        if (selected is null)
        {
            return NotFound();
        }

        // load data into the view model
        var browseData = new BrowsePapersViewModel
        {
            SelectedSubject = selected
        };

        return View(browseData);
    }

    #region Assessment
    [Route(Routes.Application.Assessment)]
    [ActionName(nameof(Routes.Application.Assessment))]
    public async Task<IActionResult> BrowseAssessmentAsync(string? curriculum, string subject, string standard)
    {
        curriculum = curriculum?.ToUpper(); // keep in sync with AppController browse action

        // perform same checks as above
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
            return await GetAssessmentResultAsync<NceaResource, NceaAssessmentViewModel>(context, standard, selected, GetNceaResourceAsync, "NceaResource");
        }
        else if (source == CurriculumSource.Cambridge)
        {
            return await GetAssessmentResultAsync<CambridgeResource, CambridgeAssessmentViewModel>(context, standard, selected, GetCambridgeResourceAsync, "CambridgeResource");
        }

        throw new UnreachableException();
    }

    internal async ValueTask<IActionResult> GetAssessmentResultAsync<TResource, TViewModel>(ApplicationDbContext context, string standard, Subject subject, 
        Func<ApplicationDbContext, string, Subject, ValueTask<TResource?>> find, string viewName)
        where TResource : CustomResource
        where TViewModel : AssessmentViewModel<TResource>, new()
    {
        var resource = await find(context, standard, subject);

        if (resource is null)
        {
            return NotFound();
        }

        return View(viewName, new TViewModel()
        {
             Subject = (SubjectViewModel)subject,
             Resource = resource
        });
    }

    internal static async ValueTask<NceaResource?> GetNceaResourceAsync(ApplicationDbContext context, string standard, Subject subject)
    {
        Debug.Assert(subject != null);

        if (!int.TryParse(standard, out int number))
        {
            return null;
        }

        var resource = await Resources.FindNceaResourceByAchievementStandardAsync(context, number);

        if (resource is null)
        {
            return null;
        }

        return subject.NceaResource.Any(r => r.Id == resource.Id) ? resource : null;
    }

    internal static async ValueTask<CambridgeResource?> GetCambridgeResourceAsync(ApplicationDbContext context, string standard, Subject subject)
    {
        Debug.Assert(subject != null);

        if (!Cambridge.TryParseResource(standard, out var number, out var season, out var variant))
        {
            return null;
        }

        var resource = await Resources.FindCambridgeResourceByIdentifersAsync(context, number, season, variant);

        if (resource is null)
        {
            return null;
        }

        // TODO: Cambridge requires extra checks for now. Change additions to db to fix
        return subject.CambridgeResource.Any(r => r.Number == number && r.Season == season && r.Variant == variant) ? resource : null;
    }
    #endregion

    [Route(Routes.Application.PaperViewer)]
    [ActionName(nameof(Routes.Application.PaperViewer))]
    public async Task<IActionResult> ViewPaperAsync(string? curriculum, string subject, string standard, string year)
    {
        curriculum = curriculum?.ToUpper(); // keep in sync with AppController browse action

        if (!Curriculum.TryParse(curriculum, out var level, out var source) || string.IsNullOrWhiteSpace(subject) || !int.TryParse(year.AsSpan(), out var yearValue))
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

            if (nceaResource == null || !selected.NceaResource.Any(r => r.Id == nceaResource.Id) && !nceaResource.Items.Any(i => i.Year == yearValue))
            {
                return NotFound();
            }

            return View("NceaPaperViewer", new NceaItemViewModel()
            {
                Item = nceaResource.Items.First(i => i.Year == yearValue),
                Resource = nceaResource,
                Subject = (SubjectViewModel)selected
            });
        }
        else if (source == CurriculumSource.Cambridge)
        {
            if (!Cambridge.TryParseResource(standard, out var number, out var season, out var variant))
            {
                return NotFound();
            }

            var cambridgeResource = await Resources.FindCambridgeResourceByIdentifersAsync(context, number, season, variant);

            if (cambridgeResource == null || !selected.CambridgeResource.Any(r => r.Number == number && r.Season == season && r.Variant == variant) && !cambridgeResource.Items.Any(i => i.Year == yearValue))
            {
                return NotFound();
            }

            return View("CambridgePaperViewer", new CambridgeItemViewModel()
            {
                Item = cambridgeResource.Items.First(i => i.Year == yearValue),
                Resource = cambridgeResource,
                Subject = (SubjectViewModel)selected
            });
        }

        throw new UnreachableException();
    }
}
