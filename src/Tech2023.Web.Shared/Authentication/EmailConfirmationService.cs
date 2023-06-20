using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using Tech2023.DAL;
using Tech2023.Web.Shared.Email;

namespace Tech2023.Web.Shared.Authentication;

/// <summary>
/// The default service used to send email confirmations to users for their accounts
/// </summary>
public class EmailConfirmationService : IEmailConfirmationService<ApplicationUser>
{
    internal readonly ILogger<IEmailConfirmationService<ApplicationUser>> _logger;
    internal readonly IEmailClient _emailClient;
    internal readonly UserManager<ApplicationUser> _userManager;

    public EmailConfirmationService(ILogger<IEmailConfirmationService<ApplicationUser>> logger, IEmailClient emailClient, 
        UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _emailClient = emailClient;
        _userManager = userManager;
    }

    /// <summary>
    /// Sends an email to the specified user that provides the url for authentication, this method a delegate that creates the url string using the specified code
    /// </summary>
    /// <param name="user">The user to send the email to</param>
    /// <param name="urlCreate">Creates the url</param>
    /// <returns>Whether the email was sent, or if the function was interrupted</returns>
    public async Task<bool> SendEmailConfirmationAsync(ApplicationUser user, Func<string, string> urlCreate)
    {
        ArgumentNullException.ThrowIfNull(urlCreate);
        ArgumentNullException.ThrowIfNull(user);

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);

        if (!WebEncoderHelpers.TryEncodeToUtf8Base64Url(token, out token))
        {
            return false;
        }

        var url = urlCreate(token);

        await _emailClient.SendEmailAsync(user.Email!, "Confirm Your Email", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(url)}'>clicking here</a>.");

        return true;
    }
}
