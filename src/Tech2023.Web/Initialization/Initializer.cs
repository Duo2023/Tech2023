using System.Diagnostics;
using System.Text.Json;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Tech2023.DAL;
using Tech2023.DAL.Models;
using Tech2023.DAL.Queries;

namespace Tech2023.Web.Initialization;

internal class Initializer : IDataInitializer
{
    internal readonly RoleManager<ApplicationRole> _roleManager;
    internal readonly UserManager<ApplicationUser> _userManager;
    internal readonly ILogger<IDataInitializer> _logger;
    internal readonly IDbContextFactory<ApplicationDbContext> _factory;
    internal readonly IConfiguration _configuration;

    public Initializer(IServiceProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);

        _roleManager = provider.GetRequiredService<RoleManager<ApplicationRole>>();
        _userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
        _logger = provider.GetRequiredService<ILogger<IDataInitializer>>();
        _factory = provider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
        _configuration = provider.GetRequiredService<IConfiguration>();
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


    internal async Task CreateSubjectsAsync(ApplicationDbContext context)
    {
        var basePath = _configuration["SeedPath"];

        if (basePath is null)
        {
            _logger.LogError("Base Path Not Defined");
            return;
        }

        string path = Path.Combine(basePath, "subjects.json");

        if (!File.Exists(path))
        {
            _logger.LogError("JSON data path invalid, no file");
            return;
        }

        using var stream = File.OpenRead(path);

        SubjectJsonModel[] subjects;

        try
        {
            subjects = JsonSerializer.Deserialize(stream, SeedSerializationContext.Default.SubjectJsonModelArray) ?? throw new InvalidOperationException();
        }
        catch
        {
            _logger.LogError("Debug data failed to be read JSON or file error");
            return;
        }

        var values = subjects.Select(s => (Subject)s);

        await Subjects.CreateSubjectsAsync(context, values);
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
                foreach (var _ in Enumerable.Range(0, Random.Shared.Next(1, 10)))
                {
                    item.CambridgeResource?.Add(GenerateCambridgeResource());
                }
            }
        }

        context.UpdateRange(subjects);

        await context.SaveChangesAsync();
    }

    internal static NceaResource GenerateNceaStandard()
    {
        var resource = new NceaResource()
        {
            AssessmentType = (NceaAssessmentType)Random.Shared.Next((int)NceaAssessmentType.Internal, (int)NceaAssessmentType.Unit),
            AchievementStandard = Random.Shared.Next(1, ushort.MaxValue),
            Description = "An achievement standard",
            Created = DateTimeOffset.UtcNow,
        };

        resource.SyncUpdated();

        return resource;
    }

    internal static CambridgeResource GenerateCambridgeResource()
    {
        var resource = new CambridgeResource()
        {
            Season = (Season)Random.Shared.Next((int)Season.Spring, (int)Season.Winter),
            Variant = (Variant)Random.Shared.Next((int)Variant.One, (int)Variant.Three),
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

            user.SavedSubjects.AddRange(subjects.OrderBy(s => Random.Shared.Next()).Take(5));

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

        if (!result.Succeeded)
        {
            _logger.LogError("Sudo user failed to create: does it already exist?");
            return;
        }

        await _userManager.AddToRoleAsync(user, Roles.Administrator);
    }


#endif
}
