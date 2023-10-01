using System.Net;
using System.Diagnostics;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Tech2023.DAL;
using Tech2023.DAL.Queries;
using Tech2023.Web.Caching;
using Tech2023.Web.ViewModels;
using Azure.Core;
using Tech2023.DAL.Identity;

namespace Tech2023.Web.Controllers;

/// <summary>
/// Provides methods to manipulate the users subjects collections
/// </summary>
[Authorize]
public class SubjectsController : Controller
{
    internal readonly IDbContextFactory<ApplicationDbContext> _factory;
    internal readonly ILogger<SubjectsController> _logger;
    internal readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubjectsController"/> class
    /// </summary>
    public SubjectsController(IDbContextFactory<ApplicationDbContext> factory,  ILogger<SubjectsController> logger, UserManager<ApplicationUser> userManager)
    {
        _factory = factory;
        _logger = logger;
        _userManager = userManager;
    }

    [HttpPost]
    [Route(Routes.Subjects.Delete)]
    public async Task<IActionResult> DeleteAsync([FromQuery] Guid id)
    {
        using var context = await _factory.CreateDbContextAsync();
        
        // TODO: Convert this query into Users.* query assembly class however if we convert this to use the usermanager table the delete does not go through
#nullable disable
        var user = await context.Users
            .Include(u => u.SavedSubjects)
            .Where(u => u.NormalizedUserName == _userManager.NormalizeEmail(User.Identity.Name))
            .FirstAsync();

        int count = user.SavedSubjects.RemoveAll(s => s.Id == id);

        if (count > 0)
        {
            await context.SaveChangesAsync();
        } 

        return Redirect(Request.Headers["Referer"].ToString() ?? Routes.Application.Home);
    }

    [HttpGet]
    [Route(Routes.Subjects.Add)]
    public async Task<IActionResult> AddAsync([FromQuery] Guid id)
    {
        using var context = await _factory.CreateDbContextAsync();
        
#nullable disable
        var user = await context.Users
            .Include(u => u.SavedSubjects)
            .Where(u => u.NormalizedUserName == _userManager.NormalizeEmail(User.Identity.Name))
            .FirstAsync();

        var subject = await context.Subjects.FindAsync(id);

        if (subject is not null && !user.SavedSubjects.Contains(subject))
        {
            user.SavedSubjects.Add(subject);

            await context.SaveChangesAsync();
        }

        return Redirect(Request.Headers["Referer"].ToString() ?? Routes.Application.Home);
    }
}
