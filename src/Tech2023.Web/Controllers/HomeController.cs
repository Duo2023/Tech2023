using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Tech2023.DAL.Models;
using Tech2023.Web.Models;
using Tech2023.Web.Shared;

namespace Tech2023.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHttpClientFactory _factory;
    private static readonly Lazy<PrivacyPolicy> _errorPrivacyPolicy = new(() => new PrivacyPolicy { Content = "Couldn't get Privacy Policy" });

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

    [Route(Routes.Privacy)]
    public async Task<IActionResult> Privacy()
    {
        var client = _factory.CreateClient(Clients.API);

        try
        {
            var response = await client.GetAsync("/api/privacy");

            response.EnsureSuccessStatusCode();

            PrivacyPolicy? policy = await response.Content.ReadFromJsonAsync(WebSerializationContext.Default.PrivacyPolicy);

            if (policy is null)
            {
                _logger.LogError("Error parsing Privacy Policy API response!");
                return View(_errorPrivacyPolicy.Value);
            }

            return View(policy);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError("Unsuccessful Privacy Policy API response!\n\t" +
                "Status Code: {code}\n\t" +
                "Http Error: {err}", ex.StatusCode, ex.Message);

            return View(_errorPrivacyPolicy.Value);
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
