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
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _factory = factory;
    }

    [Route(Routes.Home)]
    public IActionResult Index()
    {
        return View();
    }

    [Route(Routes.Privacy)]
    public async Task<IActionResult> PrivacyAsync()
    {
        var client = _factory.CreateClient(Clients.API);

        client.Timeout = TimeSpan.FromMilliseconds(500);

        try
        {
            var response = await client.GetAsync(ApiRoutes.Privacy.Base);

            if (!response.IsSuccessStatusCode)
            {
                return Error($"Status Code: {response.StatusCode} sent back by {ApiRoutes.Privacy.Base}");
            }

            PrivacyPolicy? policy = await response.Content.ReadFromJsonAsync(WebSerializationContext.Default.PrivacyPolicy);

            if (policy is null)
            {
                return Error($"API contract broken: Privacy API");
            }

            return View(policy);
        }
        catch (HttpRequestException exception)
        {
            return Error(exception.Message);
        }
        catch (TaskCanceledException)
        {
            return Error("Task was cancelled for retrieving privacy policy because it took too long");
        }

        IActionResult Error(string message)
        {
            _logger.LogError("{error}", message);

            return View(_errorPrivacyPolicy.Value);
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
