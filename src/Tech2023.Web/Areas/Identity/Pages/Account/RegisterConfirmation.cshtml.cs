using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Tech2023.DAL;

#nullable disable

namespace Tech2023.Web.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class RegisterConfirmationModel : PageModel
{
    internal readonly UserManager<ApplicationUser> _userManager;

    public RegisterConfirmationModel(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public string Email { get; set; }

    public async Task<IActionResult> OnGetAsync(string email, string returnUrl)
    {
        if (email is null)
        {
            return RedirectToAction("/Index");
        }

        var user = await _userManager.FindByNameAsync(email);

        if (user == null)
        {
            return NotFound();
        }

        Email = email;

        return Page();
    }
}
