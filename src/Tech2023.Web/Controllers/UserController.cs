using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;

using Microsoft.AspNetCore.Mvc;

using Tech2023.Web.Shared;
using Tech2023.Web.Shared.Authentication;

namespace Tech2023.Web;

public class UserController : Controller
{
    internal readonly ILogger<AppController> _logger;
    internal readonly IHttpClientFactory _factory;

    public UserController(ILogger<AppController> logger, IHttpClientFactory factory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    [Route(Routes.User.Login)]
    public IActionResult Login()
    {
        return View();
    }

    [Route(Routes.User.Register)]
    public IActionResult Register()
    {
        return View();
    }

    [Route(Routes.User.ForgotPassword)]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [Route(Routes.User.ResetPassword)]
    public IActionResult ResetPassword()
    {
        return View();
    }

    [HttpPost]
    [ActionName("SendReset")]
    [Route(Routes.User.ForgotPassword)]
    public async Task<IActionResult> SendPasswordResetRequestAsync(EmailAddress address)
    {
        Debug.WriteLine($"Sending password reset to {address.Email}");

        var client = _factory.CreateClient(Clients.API);

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Users.ForgotPassword)
            {
                Content = new StringContent(JsonSerializer.Serialize(address, WebSerializationContext.Default.EmailAddress), Defaults.ContentTypeHeader)
            };

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return Login();
            }
        }
        catch
        {
            _logger.LogError("API Down for Password reset");
        }

        return Login();
    }
}
