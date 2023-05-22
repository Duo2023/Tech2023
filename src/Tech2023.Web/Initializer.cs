using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Tech2023.DAL;
using Tech2023.DAL.Models;

namespace Tech2023.Web;

internal class Initializer : IDataInitializer
{
    internal readonly RoleManager<ApplicationRole> _roleManager;
    internal readonly UserManager<ApplicationUser> _userManager;
    internal readonly ILogger<IDataInitializer> _logger;
    internal readonly IDbContextFactory<ApplicationDbContext> _factory;

    public Initializer(IServiceProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);

        _roleManager = provider.GetRequiredService<RoleManager<ApplicationRole>>();
        _userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
        _logger = provider.GetRequiredService<ILogger<IDataInitializer>>();
        _factory = provider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
    }

    public async Task InitializeAsync()
    {
        foreach (string role in Roles.All())
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new ApplicationRole(role));
            }
        }

        using var context = _factory.CreateDbContext();

        if (!context.PrivacyPolicies.Any())
        {
            var policy = new PrivacyPolicy()
            {
                Version = 1,
                Content = "No privacy policy has been configured",
                Created = DateTimeOffset.Now
            };

            policy.Updated = policy.Created;

            context.PrivacyPolicies.Add(policy);

            await context.SaveChangesAsync();
        }

#if DEBUG
        await CreateDebugUserAsync();
#endif
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

        var result = await _userManager.CreateAsync(user, "sudoUser555!");

        System.Diagnostics.Debug.Assert(result.Succeeded, "Debug user failed to create");

        await _userManager.AddToRoleAsync(user, Roles.Administrator);
    }
#endif
}
