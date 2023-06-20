using Microsoft.AspNetCore.Identity;

namespace Tech2023.Web.Shared.Authentication;

public interface IEmailConfirmationService<TUser> where TUser : IdentityUser<Guid>
{
    Task<bool> SendEmailConfirmationAsync(TUser user, Func<string, string> function);
}
