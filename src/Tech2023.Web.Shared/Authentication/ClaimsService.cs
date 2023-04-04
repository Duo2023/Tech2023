using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Tech2023.DAL;
using Tech2023.Web.Shared.Authentication;

namespace Tech2023.Web.Shared.Authenticaton;

/// <summary>
/// Issues out authentication claims
/// </summary>
public sealed class ClaimsService : IClaimsService
{
    internal readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClaimsService"/> class with the specified <see cref="UserManager{TUser}"/> instance
    /// </summary>
    /// <param name="userManager">The user manager which provides roles for the claims</param>
    /// <exception cref="ArgumentNullException">Thrown if the <see cref="UserManager{TUser}"/> is <see langword="null"/></exception>
    public ClaimsService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    /// <summary>
    /// Gets a list of the claims the user has
    /// </summary>
    /// <param name="user">The user account</param>
    /// <returns>A list of claims</returns>
    public async Task<List<Claim>> GetUserClaimsAsync(ApplicationUser user)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentException.ThrowIfNullOrEmpty(user.Email);

        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var roles = await _userManager.GetRolesAsync(user);

        for (int i = 0; i < roles.Count; i++)
        {
            claims.Add(new Claim(ClaimTypes.Role, roles[i]));
        }

        return claims;
    }
}
