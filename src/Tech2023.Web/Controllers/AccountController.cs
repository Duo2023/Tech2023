#nullable disable
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using Tech2023.DAL;
using Tech2023.Web.Shared;
using Tech2023.Web.Shared.Authentication;

namespace Tech2023.Web.Controllers;

/// <summary>
/// Controller used for account related actions
/// </summary>
public class AccountController : Controller
{
    internal readonly SignInManager<ApplicationUser> _signInManager;
    internal readonly UserManager<ApplicationUser> _userManager;
    internal readonly IUserStore<ApplicationUser> _userStore;
    internal readonly IUserEmailStore<ApplicationUser> _emailStore;
    internal readonly IEmailConfirmationService<ApplicationUser> _confirmationService;
    internal readonly ILogger<AccountController> _logger;

    public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
        IUserStore<ApplicationUser> userStore, IEmailConfirmationService<ApplicationUser> confirmationService, 
        ILogger<AccountController> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = (IUserEmailStore<ApplicationUser>)_userStore;
        _confirmationService = confirmationService;
        _logger = logger;
    }

    [HttpGet]
    [ActionName(nameof(Routes.Account.Login))]
    [Route(Routes.Account.Login)]
    public async Task<IActionResult> LoginAsync(string returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;

        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        return View();
    }

    [HttpPost]
    [ActionName(nameof(Routes.Account.Login))]
    public async Task<IActionResult> LoginAsync(Login login, string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, true, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return LocalRedirect(returnUrl);
            }

            // 2FA is not supported but could be added through here

            //if (result.RequiresTwoFactor)
            //{
            //    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = true });
            //}

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return Lockout();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View();
            }
        }

        _logger.LogInformation("Something failed in login");

        // If we got this far, something failed, redisplay form
        return View();
    }

    [HttpGet]
    [Route(Routes.Account.Register)]
    public IActionResult Register(string returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;

        return View();
    }

    [HttpPost]
    [ActionName(nameof(Register))]
    [Route(Routes.Account.Register)]
    public async Task<IActionResult> RegisterAsync(Register input, string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                Created = DateTimeOffset.UtcNow
            };

            user.Updated = user.Created;

            await _userStore.SetUserNameAsync(user, input.Email, CancellationToken.None);

            await _emailStore.SetEmailAsync(user, input.Email, CancellationToken.None);

            var result = await _userManager.CreateAsync(user, input.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                var userId = await _userManager.GetUserIdAsync(user);

                var emailSuccess = await _confirmationService.SendEmailConfirmationAsync(user,
                    (code) => Url.Action(nameof(Routes.Account.ConfirmEmail), controller: "Account", new { userId, code }, Request.Scheme));

                if (!emailSuccess)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                return Redirect($"{Routes.Account.RegisterConfirmation}?email={input.Email}");

            }
            foreach (var error in result.Errors)
            {
                if (error.Code == "DuplicateUserName") // workaround for https://github.com/Duo2023/Tech2023/issues/33 should not be kept
                {
                    continue;
                }

                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View();
    }

    [HttpGet]
    [Route(Routes.Account.RegisterConfirmation)]
    public async Task<IActionResult> RegisterConfirmationAsync([FromQuery] string email)
    {
        Debug.WriteLine($"{Routes.Account.RegisterConfirmation} accessed in Debug with {email}");

        if (email is null)
        {
            return Redirect(Routes.Application.Home);
        }

        var user = await _userManager.FindByNameAsync(email);

        if (user is null)
        {
            return NotFound();
        }

        return View("RegisterConfirmation", email);
    }

    [HttpGet]
    [ActionName(nameof(Routes.Account.ConfirmEmail))]
    [Route(Routes.Account.ConfirmEmail)]
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
    {
        const string Error = "Error confirming your email";
        const string Success = "Thank you for confirming your email";

        string message = Error;

        if (userId is null || code is null)
        {
            goto Exit;
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            goto Exit;
        }

        if (!WebEncoderHelpers.TryDecodeFromBase64UrlEncoded(code, out string output))
        {
            goto Exit;
        }

        var result = await _userManager.ConfirmEmailAsync(user, output);

        if (result.Succeeded)
        {
            message = Success;
        }

    Exit:
        return View("ConfirmEmail", message);
    }

    [HttpGet]
    [Route(Routes.Account.ResendEmailConfirmation)]
    public IActionResult ResendEmailConfirmation()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ResendEmailConfirmationAsync(EmailAddress address)
    {
        if (!ModelState.IsValid)
        {
            return View(nameof(ResendEmailConfirmation));
        }

        var user = await _userManager.FindByEmailAsync(address.Email);

        if (user is null)
        {
            return View(nameof(ResendEmailConfirmation));
        }

        var userId = await _userManager.GetUserIdAsync(user);

        var emailSuccess = await _confirmationService.SendEmailConfirmationAsync(user,
            (code) => Url.Action(nameof(Routes.Account.ConfirmEmail), controller: "Account", new { userId, code }, Request.Scheme));

        if (!emailSuccess)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return RedirectToPage("/Account/Login");
    }

    [Route(Routes.Account.ForgotPassword)]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [AllowAnonymous]
    [Route(Routes.Account.Lockout)]
    public IActionResult Lockout()
    {
        return View(); // returns Lockout view
    }

    [HttpPost]
    [ActionName("Logout")]
    [Route("logout")]
    public async Task<IActionResult> LogoutAsync(string returnUrl)
    {
        await _signInManager.SignOutAsync();

        Debug.WriteLine("user signed out");

        if (returnUrl != null)
        {
            return LocalRedirect(returnUrl);
        }
        else
        {
            // This needs to be a redirect so that the browser performs a new
            // request and the identity for the user gets updated.
            return RedirectToPage("/");
        }
    }
}
