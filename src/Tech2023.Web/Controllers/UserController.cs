using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;

using Microsoft.AspNetCore.Mvc;

using Tech2023.Web.Shared;
using Tech2023.Web.Shared.Authentication;

namespace Tech2023.Web;

/// <summary>
/// User controller that creates 
/// </summary>
public class UserController : Controller
{
    internal readonly ILogger<AppController> _logger;
    internal readonly IHttpClientFactory _factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserController"/> class
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="factory"></param>
    /// <exception cref="ArgumentNullException">Thrown if either parameters are null</exception>
    public UserController(ILogger<AppController> logger, IHttpClientFactory factory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    /// <summary>
    /// Returns the login view
    /// </summary>
    [Route(Routes.User.Login)]
    public IActionResult Login()
    {
        return View();
    }

    /// <summary>
    /// Returns the register view
    /// </summary>
    [Route(Routes.User.Register)]
    public IActionResult Register()
    {
        return View();
    }

    /// <summary>
    /// Returns the forgot password view
    /// </summary>
    [Route(Routes.User.ForgotPassword)]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    /// <summary>
    /// Returns the reset password view
    /// </summary>
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
