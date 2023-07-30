using Microsoft.AspNetCore.Mvc;

/*
 * Like most of the other controllers in this repository it is backed by the web api on the other side, our web controller calls the Web API
 */

namespace Tech2023.Web.Controllers;

/// <summary>
/// Settings controller to control the configure client side settings and account wide settings
/// </summary>
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
