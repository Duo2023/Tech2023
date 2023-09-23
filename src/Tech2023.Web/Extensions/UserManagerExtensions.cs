using System.Diagnostics;
using System.Security.Claims;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Tech2023.DAL;
using Tech2023.Web.Models;

namespace Tech2023.Web.Extensions;

internal static class UserManagerExtensions
{
    public static async Task<ApplicationUser> FindByUserAsync(this UserManager<ApplicationUser> userManager, ClaimsPrincipal principal)
    {
        Debug.Assert(userManager != null);

#nullable disable

       string userName = userManager.NormalizeEmail(principal.Identity.Name);

        return await userManager.Users
            .Include(user => user.SavedSubjects)
            .SingleOrDefaultAsync(u => u.NormalizedUserName == userName);
    }
}
