using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

using Tech2023.DAL;

namespace Tech2023.Web.Controllers;

public class AccountController : Controller
{
    internal readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
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
