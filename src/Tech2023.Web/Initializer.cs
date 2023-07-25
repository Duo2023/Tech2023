using System.Diagnostics;

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
        await CreateDebuggingDataAsync(context);
#endif
    }

#if DEBUG
    internal async Task CreateDebuggingDataAsync(ApplicationDbContext context)
    {
        await CreateSubjectsAsync(context);
        await CreateDebugUserAsync(context);
    }

    internal async Task CreateDebugUserAsync(ApplicationDbContext context)
    {
        const string Username = "sudo@sudo.com";

        ApplicationUser user = new()
        {
            Id = Guid.NewGuid(),
            Email = Username,
            UserName = Username,
            EmailConfirmed = true,
            Created = DateTimeOffset.UtcNow
        };

        user.Updated = user.Created;

        //foreach (var item in context.Subjects)
        //{
        //    user.SavedSubjects.Add(item);
        //}

        var result = await _userManager.CreateAsync(user, "sudoUser555!");

        Debug.Assert(result.Succeeded, "Debug user failed to create");

        await _userManager.AddToRoleAsync(user, Roles.Administrator);
    }

    internal static async Task CreateSubjectsAsync(ApplicationDbContext context)
    {
        await Queries.Subjects.CreateSubjectAsync(context, new Subject()
        {
            Source = CurriculumSource.Cambridge,
            Name = "Maths",
        });

        await Queries.Subjects.CreateSubjectAsync(context, new Subject()
        {
            Source = CurriculumSource.Ncea,
            Name = "Maths",
        });

        await context.SaveChangesAsync();
    }

    internal static Subject CreateSubject(string name, CurriculumSource source)
    {
        return new()
        {
            Name = name,
            Source = source,
            DisplayColor = (uint)Random.Shared.Next()
        };
    }
#endif
}
