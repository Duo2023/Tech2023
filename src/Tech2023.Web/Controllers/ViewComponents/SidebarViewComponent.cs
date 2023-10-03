using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Tech2023.DAL;
using Tech2023.Web.Caching;
using Tech2023.Web.ViewModels;
using Tech2023.DAL.Queries;
using Tech2023.DAL.Identity;

namespace Tech2023.Web.ViewComponents;

/// <summary>
/// Provides support for the left panel sidebar in the application
/// </summary>
[ViewComponent(Name = Name)]
public class SidebarViewComponent : ViewComponent
{
    internal readonly IDbContextFactory<ApplicationDbContext> _factory;
    internal readonly IMemoryCache _cache;
    internal readonly ILogger<SidebarViewComponent> _logger;
    internal readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="SidebarViewComponent"/> <see langword="class"/>
    /// </summary>
    /// <param name="userManager">User manager to access user data</param>
    public SidebarViewComponent(IDbContextFactory<ApplicationDbContext> factory, IMemoryCache cache, ILogger<SidebarViewComponent> logger, UserManager<ApplicationUser> userManager)
    {
        _factory = factory;
        _cache = cache;
        _logger = logger;
        _userManager = userManager;
    }

    public const string Name = "Sidebar";

    /// <summary>
    /// Invokes the view component using the specified state provided
    /// </summary>
    public async Task<IViewComponentResult> InvokeAsync(BrowsePapersViewModel? browseData = null, List<SubjectViewModel>? subjects = null)
    {
        subjects ??= await Users.GetUserSavedSubjectsAsViewModelsAsync(_userManager, UserClaimsPrincipal);

        List<SubjectViewModel> allSubjects = await CachedQueries.GetSubjectViewModelsFromCacheOrFetchAsync(_factory, _cache);

        var sidebarData = new SidebarViewModel
        {
            SavedSubjects = subjects,
            AllSubjects = allSubjects,
            BrowseData = browseData
        };

        // Implement filter stuff
        return View(sidebarData);
    }
}
