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
    internal readonly IEmailConfirmationService<ApplicationUser> _confirmationService;
    internal readonly UserManager<ApplicationUser> _userManager;

    public ResendEmailConfirmationModel(IEmailConfirmationService<ApplicationUser> confirmationService, UserManager<ApplicationUser> userManager)
    {
        _confirmationService = confirmationService ?? throw new ArgumentNullException(nameof(confirmationService));
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

        var userId = await _userManager.GetUserIdAsync(user);

        var emailSuccess = await _confirmationService.SendEmailConfirmationAsync(user,
                    (code) => Url.Page("/Account/ConfirmEmail", pageHandler: null, values: new { area = "Identity", userId, code, returnUrl = Url.Content("~/") }, Request.Scheme));

        if (!emailSuccess)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return RedirectToPage("/Account/Login");
    }
}
