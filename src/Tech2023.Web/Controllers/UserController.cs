using Microsoft.AspNetCore.Mvc;

namespace Tech2023.Web;

public class UserController : Controller
{
    internal readonly ILogger<AppController> _logger;

    public UserController(ILogger<AppController> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
}
