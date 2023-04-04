using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Tech2023.Web.Models;

namespace Tech2023.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [Route(Routes.Home)]
    public IActionResult Index()
    {
        return View();
    }

    [Route(Routes.App)]
    public IActionResult App()
    {
        return View();
    }

    [Route(Routes.Privacy)]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
