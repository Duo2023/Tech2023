using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using MimeKit.Cryptography;

using Tech2023.DAL;
using Tech2023.DAL.Identity;
using Tech2023.DAL.Models;
using Tech2023.DAL.Queries;
using Tech2023.Web.Initialization.Json;
using Tech2023.Web.Initialization.Json.Models;
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
    internal readonly IRoleInitializer _roleInitialzer;

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
        _roleInitialzer = provider.GetRequiredService<IRoleInitializer>();
    }

    /// <inheritdoc/>
    public async Task InitializeAsync()
    {
        await _roleInitialzer.InitializeAsync(_roleManager);

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
                Created = DateTimeOffset.Now
            };

            policy.SyncUpdated();

            context.PrivacyPolicies.Add(policy);

            await context.SaveChangesAsync();
        }
    }
}
