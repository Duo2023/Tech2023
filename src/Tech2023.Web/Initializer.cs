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
            Email = Username,
            UserName = Username,
            EmailConfirmed = true,
            Created = DateTimeOffset.UtcNow
        };

        // TODO: Create a custom UserManager to perform this and to eagerly to laod it

        //await foreach (var item in context.Subjects)
        //{
        //    user.SavedSubjects.Add(item);
        //}

        user.Updated = user.Created;

        var result = await _userManager.CreateAsync(user, "sudoUser555!");

        System.Diagnostics.Debug.Assert(result.Succeeded, "Debug user failed to create");

        await _userManager.AddToRoleAsync(user, Roles.Administrator);
    }

    internal async Task CreateSubjectsAsync(ApplicationDbContext context)
    {
        List<Subject> subjects = new(5)
        {
            CreateSubject("Maths", CurriculumSource.Ncea),
            CreateSubject("English", CurriculumSource.Ncea),
            CreateSubject("Spanish", CurriculumSource.Ncea),
            CreateSubject("English", CurriculumSource.Cambridge),
            CreateSubject("Physics", CurriculumSource.Cambridge)
        };

        context.Subjects.AddRange(subjects);

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
