using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Tech2023.Core;
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
        // use the role initialzer to create all the roles
        await _roleInitializer.InitializeAsync(_roleManager);

        using var context = await _factory.CreateDbContextAsync();

        // create the privacy policy
        await CreatePrivacyPolicyAsync(context);

        // if in debug generate all the data

#if DEBUG
        await CreateDebuggingDataAsync(context);
#endif
    }

    internal string GetFileAndValidate(string key, string extension)
    {
        // load the provided path by key
        string? file = _configuration[key];

        if (file is null)  // if null throw
        {
            Throw();
        }

        string targetExtension = Path.GetExtension(file); // get the extension of the file

        if (!targetExtension.SequenceEqual(extension))  // check the extension is the desired extension
        {
            Throw();
        }

        if (!File.Exists(file))  // check whether the file exists
        {
            Throw();
        }

        return file; // return the file

        [DoesNotReturn]
        void Throw() => throw new ConfigurationException($"The value at the configuration key '{key}' should have a valid file name ending with '{extension}' and exist at the location");
    }

    internal async Task CreatePrivacyPolicyAsync(ApplicationDbContext context)
    {
        if (!context.PrivacyPolicies.Any())
        {
            string file = GetFileAndValidate("Policy", ".txt");

            string content = File.ReadAllText(file);

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

    // this method supports the initializer for creating users using the parameters, it does not throw exceptions, but will log errors and return silently
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
            var roleAddResult = await _userManager.AddToRoleAsync(user, Roles.Administrator);

            roleAddResult.Errors.ForEach((error) => Debug.WriteLine("An error occured adding a seeded user to admin role: {0}", error));
        }
    }
}
