using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Tech2023.DAL;
using Tech2023.DAL.Models;

namespace Tech2023.Web;

/// <summary>
/// The app controller, serves most of the main content of the application
/// </summary>
[Authorize]
public class AppController : Controller
{
    internal readonly ILogger<AppController> _logger;
    internal readonly IDbContextFactory<ApplicationDbContext> _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppController"/>
    /// </summary>
    /// <param name="logger"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public AppController(ILogger<AppController> logger, IDbContextFactory<ApplicationDbContext> context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context;
    }

    [Route(Routes.Application.Home)]
    public IActionResult Home()
    {
        return View();
    }

    [Route(Routes.Application.PaperViewer)]
    public IActionResult PaperViewer()
    {
        return View();
    }

    [Route(Routes.Application.PaperBrowser)]
    public async Task<IActionResult> PaperBrowser(string curriculum, string subject)
    {
        if (!Enum.TryParse<CurriculumSource>(curriculum.AsSpan(), ignoreCase: true, out var source) || subject == null)
        {
            return NotFound();
        }

        using var context = await _context.CreateDbContextAsync();

        subject = subject.ToUpper();

        var selected = await context.Subjects
            .Where(s => s.Source == source)
            .Where(s => s.Name == subject)
            .FirstOrDefaultAsync();

        if (selected is null)
        {
            return NotFound();
        }

        return View(selected);
    }
}
