using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tech2023.DAL.Models;

namespace Tech2023.Web;

/// <summary>
/// The app controller, serves most of the main content of the application
/// </summary>
[Authorize]
public class AppController : Controller
{
    internal readonly ILogger<AppController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppController"/>
    /// </summary>
    /// <param name="logger"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public AppController(ILogger<AppController> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
    public IActionResult PaperBrowser(string curriculum, string subject)
    {
        if (curriculum == "ncea" && subject == "maths") {
            var selectedSubject = new Subject
            {
                Name = "Maths",
                Source = CurriculumSource.Ncea,
            };
            return View(selectedSubject);
        }
        return NotFound();
    }
}
