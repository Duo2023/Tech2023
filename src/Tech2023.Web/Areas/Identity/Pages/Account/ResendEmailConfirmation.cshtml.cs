using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Tech2023.DAL;
using Tech2023.Web.Shared.Authentication;
using Tech2023.Web.Shared.Email;

#nullable disable

namespace Tech2023.Web.Areas.Identity.Pages.Account;

public class ResendEmailConfirmationModel : PageModel
{
    internal readonly IEmailClient _emailClient;
    internal readonly UserManager<ApplicationUser> _userManager;

    public ResendEmailConfirmationModel(IEmailClient emailClient, UserManager<ApplicationUser> userManager)
    {
        _emailClient = emailClient ?? throw new ArgumentNullException(nameof(emailClient));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }
        
    [BindProperty]
    public EmailAddress Input { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userManager.FindByEmailAsync(Input.Email);

        if (user is null)
        {
            return Page();
        }

        return RedirectToPage("/Account/Login");
    }
}
