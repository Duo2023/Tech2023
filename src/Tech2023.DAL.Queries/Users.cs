using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tech2023.Web.ViewModels;

namespace Tech2023.DAL.Queries;

/// <summary>
/// Contains queries used to manipulate data for users
/// </summary>
public static class Users
{
#nullable disable // The caller of this method will always have a valid claims principal
    /// <summary>
    /// Finds a <see cref="ApplicationUser"/> by a <see cref="ClaimsPrincipal"/>
    /// </summary>
    public static async Task<ApplicationUser> FindByPrincipalAsync(this UserManager<ApplicationUser> userManager, ClaimsPrincipal principal)
    {
        string userName = userManager.NormalizeEmail(principal.Identity.Name);

        return await userManager.Users
            .Where(u => u.NormalizedUserName == userName)
            .Include(user => user.SavedSubjects)
            .FirstOrDefaultAsync();
    }

    public static async Task<List<SubjectViewModel>> GetUserSavedSubjectsAsViewModelsAsync(UserManager<ApplicationUser> userManager, ClaimsPrincipal principal)
    {
        string userName = userManager.NormalizeEmail(principal.Identity.Name);

        return await userManager.Users.Where(s => s.NormalizedUserName == userName)
            .Include(s => s.SavedSubjects)
            .Select(s => s.SavedSubjects)
            .SelectMany(list => list)
            .Select(s => (SubjectViewModel)s)
            .ToListAsync();
    }

    public static async Task RemoveSubjectFromUserAsync(UserManager<ApplicationUser> userManager, ClaimsPrincipal principal, Guid subjectId)
    {
        string userName = userManager.NormalizeEmail(principal.Identity.Name);


    }

#nullable restore
}
