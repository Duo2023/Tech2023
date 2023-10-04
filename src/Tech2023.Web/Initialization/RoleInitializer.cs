using Microsoft.AspNetCore.Identity;

using Tech2023.DAL.Identity;

namespace Tech2023.Web.Initialization;

/// <inheritdoc/>
internal class RoleInitializer : IRoleInitializer
{
    /// <inheritdoc/>
    public async Task InitializeAsync(RoleManager<ApplicationRole> roleManager)
    {
        ArgumentNullException.ThrowIfNull(roleManager);

        foreach (string role in Roles.All())
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new ApplicationRole(role));
            }
        }
    }
}
