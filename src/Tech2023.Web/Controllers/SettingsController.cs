using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tech2023.Web.Controllers;

/// <summary>
/// Settings controller to control the configure client side settings and account wide settings
/// </summary>
[Authorize]
public class SettingsController : Controller
{
    /// <summary>
    /// Returns the index home page for the settings page
    /// </summary>
    [Route(Routes.Settings.Home)]
    public IActionResult Index()
    {
        return View();
    }
}
