using System.Diagnostics;

using Microsoft.AspNetCore.Identity;

using Tech2023.Core;
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
                var result = await roleManager.CreateAsync(new ApplicationRole(role));

                result.Errors.ForEach((error) => Debug.WriteLine("Error occured created roles: {0}", error));
            }
        }
    }
}
