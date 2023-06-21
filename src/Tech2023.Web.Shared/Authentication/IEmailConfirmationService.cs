using Microsoft.AspNetCore.Identity;

namespace Tech2023.Web.Shared.Authentication;

/// <summary>
/// Email confirmation service, one use of this service is to authenticate the email of a user account
/// </summary>
/// <typeparam name="TUser">The <see cref="IdentityUser{TKey}"/> implementation</typeparam>
public interface IEmailConfirmationService<TUser> where TUser : IdentityUser<Guid>
{
    /// <summary>
    /// Sends the email confirmation using the url creation method provided with a token
    /// </summary>
    /// <param name="user">The user to perform the confirmation with</param>
    /// <param name="urlCreate">Takes a code and returns a url string to be used in the email</param>
    /// <returns>A flag which indicates success or failure</returns>
    Task<bool> SendEmailConfirmationAsync(TUser user, Func<string, string> urlCreate);
}
