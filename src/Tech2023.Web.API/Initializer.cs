using Microsoft.AspNetCore.Identity;
using Tech2023.DAL;

namespace Tech2023.Web.API;

internal class Initializer : IDataInitializer
{
    internal readonly RoleManager<ApplicationRole> _roleManager;
    internal readonly UserManager<ApplicationUser> _userManager;
    internal readonly ILogger<IDataInitializer> _logger;

    public Initializer(IServiceProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);

        _roleManager = provider.GetRequiredService<RoleManager<ApplicationRole>>();
        _userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
        _logger = provider.GetRequiredService<ILogger<IDataInitializer>>();
    }

    public async Task InitializeAsync()
    {
#if DEBUG
        await CreateDebugUserAsync();
#endif

        foreach (string role in Roles.All())
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new ApplicationRole(role));
            }
        }
    }

#if DEBUG
    internal async Task CreateDebugUserAsync()
    {
        const string Username = "sudo@sudo.com";

        ApplicationUser user = new()
        {
            Email = Username,
            UserName = Username,
            EmailConfirmed = true,
            Created = DateTimeOffset.UtcNow
        };

        var result = await _userManager.CreateAsync(user, "sudo");

        System.Diagnostics.Debug.Assert(result.Succeeded, "Debug user failed to create");

        await _userManager.AddToRoleAsync(user, Roles.Administrator);
    }
#endif
}
