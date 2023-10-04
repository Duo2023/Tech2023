using Microsoft.AspNetCore.Identity;
using Tech2023.DAL.Identity;

namespace Tech2023.Web.Initialization;

/// <summary>
/// Initializer for role system
/// </summary>
internal interface IRoleInitializer
{
    /// <summary>
    /// Initializes roles in an async manner
    /// </summary>
    Task InitializeAsync(RoleManager<ApplicationRole> roleManager);
}
