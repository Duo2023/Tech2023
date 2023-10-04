using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Tech2023.DAL;
using Tech2023.DAL.Identity;
using Tech2023.DAL.Models;
using Tech2023.Web.Shared;

namespace Tech2023.Web.Initialization;

/// <summary>
/// Initializes the database in the application
/// </summary>
internal partial class Initializer : IDataInitializer
{
    internal readonly RoleManager<ApplicationRole> _roleManager;
    internal readonly UserManager<ApplicationUser> _userManager;
    internal readonly ILogger<IDataInitializer> _logger;
    internal readonly IDbContextFactory<ApplicationDbContext> _factory;
    internal readonly IConfiguration _configuration;
    internal readonly IWebHostEnvironment _environment;
    internal readonly IRoleInitializer _roleInitializer;

    /// <summary>
    /// Initializes a new instance of the <see cref="Initializer"/> class
    /// </summary>
    /// <param name="provider">Provider used to initialize services for the initializer</param>
    public Initializer(IServiceProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);

        _roleManager = provider.GetRequiredService<RoleManager<ApplicationRole>>();
        _userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
        _logger = provider.GetRequiredService<ILogger<IDataInitializer>>();
        _factory = provider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
        _configuration = provider.GetRequiredService<IConfiguration>();
        _environment = provider.GetRequiredService<IWebHostEnvironment>();
        _roleInitializer = provider.GetRequiredService<IRoleInitializer>();
    }

    /// <inheritdoc/>
    public async Task InitializeAsync()
    {
        await _roleInitializer.InitializeAsync(_roleManager);

        using var context = await _factory.CreateDbContextAsync();

        await CreatePrivacyPolicyAsync(context);

#if DEBUG
        await CreateDebuggingDataAsync(context);
#endif
    }


    internal async Task CreatePrivacyPolicyAsync(ApplicationDbContext context)
    {
        if (!context.PrivacyPolicies.Any())
        {
            string? fileName = _configuration["Policy"] ?? throw new ConfigurationException("The privacy policy should have a file provided at the key 'Policy'");

            if (!File.Exists(fileName) && Path.GetExtension(fileName.AsSpan()) != ".txt")
            {
                throw new ConfigurationException("The file should exist and end with the extension .txt");
            }

            string content = File.ReadAllText(fileName);

            var policy = new PrivacyPolicy()
            {
                Version = 1,
                Content = content,
            };

            policy.SetToCurrent();

            context.PrivacyPolicies.Add(policy);

            await context.SaveChangesAsync();
        }
    }

    internal async Task CreateUserAsync(string username, string password, bool admin)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            throw new ConfigurationException("Username or password of a user cannot be empty or null");
        }

        var user = new ApplicationUser()
        {
            Id = Guid.NewGuid(),
            Email = username,
            UserName = username,
            EmailConfirmed = true
        };

        user.SetToCurrent();

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            _logger.LogError("Failed to create user with {username}, does it already exist?", username);
            return;
        }

        if (admin)
        {
            await _userManager.AddToRoleAsync(user, Roles.Administrator);
        }
    }
}
