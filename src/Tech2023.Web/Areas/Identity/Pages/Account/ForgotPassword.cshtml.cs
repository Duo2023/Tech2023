using System.Text.Encodings.Web;
using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

using Tech2023.DAL;
using Tech2023.Web.Shared.Authentication;
using Tech2023.Web.Shared.Email;

#nullable disable

namespace Tech2023.Web.Areas.Identity.Pages.Account;

public class ForgotPasswordModel : PageModel
{
    internal readonly UserManager<ApplicationUser> _userManager;
    internal readonly IEmailClient _emailClient;

    public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IEmailClient emailClient)
    {
        _emailClient = emailClient;
        _userManager = userManager;
    }

    [BindProperty]
    public EmailAddress Input { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(Input.Email);

            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            // For more information on how to enable account confirmation and password reset please
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { area = "Identity", code },
                protocol: Request.Scheme);

            await _emailClient.SendEmailAsync(
                Input.Email,
                "Reset Password",
                $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            return RedirectToPage("./ForgotPasswordConfirmation");
        }

        return Page();
    }
}
