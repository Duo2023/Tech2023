using System.Diagnostics;

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
    internal readonly ILogger<AdminController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdminController"/> class
    /// </summary>
    /// <param name="logger">The logger provided as a dependency</param>
    /// <remarks>
    /// This constructor will be discovered by the DI container and it will inject the services to the parameters
    /// </remarks>
    public AdminController(ILogger<AdminController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Returns the home admin page (panel)
    /// </summary>
    [HttpGet]
    [Route(Routes.Admin.Home)]
    public IActionResult Index()
    {
        Debug.Assert(User.Identity != null); // user has to be authenticated to reach here

        _logger.LogDebug("Admin controller accessed by: {user}", User.Identity?.Name ?? "?");

        return View();
    }

    /// <summary>
    /// Returns the upload page to upload resources
    /// </summary>
    /// <returns></returns>
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
