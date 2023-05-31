using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

using System.Text;

using Tech2023.DAL;

namespace Tech2023.Web.Areas.Identity.Pages.Account;

public class ConfirmEmailModel : PageModel
{
    internal readonly UserManager<ApplicationUser> _userManager;
    internal readonly ILogger<ConfirmEmailModel> _logger;

    public ConfirmEmailModel(UserManager<ApplicationUser> userManager, ILogger<ConfirmEmailModel> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

#nullable disable
    [TempData]
    public string StatusMessage { get; set; }
#nullable restore

    public async Task<IActionResult> OnGetAsync(string userId, string code)
    {
        if (userId is null || code is null)
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return NotFound();
        }

        var url = WebEncoders.Base64UrlDecode(code);

        code = Encoding.UTF8.GetString(url);

        var result = await _userManager.ConfirmEmailAsync(user, code);

        if (result.Succeeded)
        {
            StatusMessage = "Thank you for confirming your email";
        }
        else
        {
            StatusMessage = "Error confirming your email";
        }

        return Page();
    }
}
