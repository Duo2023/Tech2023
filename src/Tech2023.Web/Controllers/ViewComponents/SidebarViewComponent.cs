using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Tech2023.DAL;
using Tech2023.Web.Models;
using Tech2023.Web.Models.Components;
using Tech2023.Web.Extensions;

namespace Tech2023.Web.ViewComponents;

/// <summary>
/// Provides support for the left panel sidebar in the application
/// </summary>
[ViewComponent(Name = Name)]
public class SidebarViewComponent : ViewComponent
{
    internal readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="SidebarViewComponent"/> <see langword="class"/>
    /// </summary>
    /// <param name="userManager">User manager to access user data</param>
    public SidebarViewComponent(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public const string Name = "Sidebar";

    /// <summary>
    /// Invokes the view component using the specified state provided
    /// </summary>
    public async Task<IViewComponentResult> InvokeAsync(BrowsePapersViewModel? browseData = null)
    {
        var user = await _userManager.FindByUserAsync(UserClaimsPrincipal);

        var sidebarData = new SidebarViewModel
        {
            Subjects = user.SavedSubjects,
            BrowseData = browseData
        };

        // Implement filter stuff
        return View(sidebarData);
    }
}
