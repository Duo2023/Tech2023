using Microsoft.AspNetCore.Mvc;

namespace Tech2023.Web;

public class AppController : Controller
{
    internal readonly ILogger<AppController> _logger;

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
