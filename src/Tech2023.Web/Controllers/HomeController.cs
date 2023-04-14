using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Tech2023.DAL.Models;
using Tech2023.Web.Models;

namespace Tech2023.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHttpClientFactory _factory;
    private PrivacyPolicy? _privacyPolicy { get; set; }

    public HomeController(ILogger<HomeController> logger, IHttpClientFactory factory)
    {
        _logger = logger;
        _factory = factory;
    }

    [Route(Routes.Home)]
    public IActionResult Index()
    {
        return View();
    }


    private async void GetPrivacyPolicy()
    {
        var client = _factory.CreateClient(Clients.API);
        var response = await client.GetAsync("/api/privacy");


        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Unsuccessful Privacy Policy API response!\n" +
                "Api Response: {res}", response);
            return;
        }

        PrivacyPolicy? policy = await response.Content.ReadFromJsonAsync<PrivacyPolicy>();
        if (policy is null)
        {
            _logger.LogError("Error parsing Privacy Policy API response!");
            return;
        }
        _privacyPolicy = policy;
    }

    [Route(Routes.Privacy)]
    public IActionResult Privacy()
    {
        GetPrivacyPolicy();
        return View(_privacyPolicy);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
