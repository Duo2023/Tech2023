using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Tech2023.DAL;

namespace Tech2023.Web.Controllers;

/// <summary>
/// Administrative controller to return views and perform admin actions
/// </summary>
[Authorize(Roles = Roles.Administrator)]
public sealed class AdminController : Controller
{
    /// <summary>
    /// Returns the home admin page (panel)
    /// </summary>
    [HttpGet]
    [Route(Routes.Admin.Home)]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Route(Routes.Admin.Upload)]
    public async Task<IActionResult> Upload()
    {
        await Task.Yield();

        return View();
    }

    [HttpPost]
    [Route(Routes.Admin.Upload)]
    public Task<IActionResult> UploadPaperAsync()
    {
        throw new NotImplementedException();
    }
}
