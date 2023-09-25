using System.Diagnostics;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Tech2023.DAL;
using Tech2023.DAL.Models;
using Tech2023.DAL.Queries;

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

            policy.SyncUpdated();

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
        await AddUsersToSubjectsAsync(context);
        await AddResourcesToSubjectsAsync(context);
    }

    internal static async Task AddResourcesToSubjectsAsync(ApplicationDbContext context)
    {
        var subjects = await context.Subjects.ToListAsync();

        foreach (var item in subjects)
        {
            if (item.Source == CurriculumSource.Ncea)
            {
                foreach (var _ in Enumerable.Range(0, Random.Shared.Next(1, 10)))
                {
                    item.NceaResource?.Add(GenerateNceaStandard());
                }
            }
            else if (item.Source == CurriculumSource.Cambridge)
            {
            }
        }

        context.UpdateRange(subjects);

        await context.SaveChangesAsync();
    }

    internal static NceaResource GenerateNceaStandard()
    {
        var resource = new NceaResource()
        {
            AssessmentType = (NceaAssessmentType)Random.Shared.Next((int)NceaAssessmentType.Unit),
            AchievementStandard = Random.Shared.Next(1, ushort.MaxValue),
            Created = DateTimeOffset.UtcNow,
        };

        resource.SyncUpdated();

        return resource;
    }
  

    internal static async Task AddUsersToSubjectsAsync(ApplicationDbContext context)
    {
        var subjects = await context.Subjects.ToListAsync();

        foreach (var user in context.Users)
        {
            user.Updated = DateTimeOffset.UtcNow;

            user.SavedSubjects.AddRange(subjects);

            context.Update(user);
        }

        await context.SaveChangesAsync();
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

        user.SyncUpdated();

        var result = await _userManager.CreateAsync(user, "sudoUser555!");

        Debug.Assert(result.Succeeded, "Debug user failed to create");

        await _userManager.AddToRoleAsync(user, Roles.Administrator);
    }

    internal static async Task CreateSubjectsAsync(ApplicationDbContext context)
    {
        await Subjects.CreateSubjectAsync(context, CreateSubject("Maths", CurriculumSource.Cambridge, CurriculumLevel.L1));

        await Subjects.CreateSubjectAsync(context, CreateSubject("Maths", CurriculumSource.Ncea, CurriculumLevel.L3));

        await context.SaveChangesAsync();
    }

    internal static Subject CreateSubject(string name, CurriculumSource source, CurriculumLevel level)
    {
        return new()
        {
            Name = name.ToUpper(),
            Source = source,
            Level = level,
            DisplayColor = (uint)Random.Shared.Next()
        };
    }
#endif
}
