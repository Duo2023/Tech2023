using Microsoft.AspNetCore.Mvc;

namespace Tech2023.Web;

/// <summary>
/// The app controller, serves most of the main content of the application
/// </summary>
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
}
