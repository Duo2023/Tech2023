using System.Diagnostics;
using System.Text.Json;
using Tech2023.DAL.Identity;
using Tech2023.DAL.Models;
using Tech2023.DAL;
using Tech2023.Web.Initialization.Json.Models;
using Tech2023.Web.Initialization.Json;
using Microsoft.EntityFrameworkCore;
using Tech2023.DAL.Queries;

namespace Tech2023.Web.Initialization;

/// <inheritdoc/>
internal partial class Initializer
{
    internal async Task CreateDebuggingDataAsync(ApplicationDbContext context)
    {
        await CreateSubjectsAsync(context);
        await CreateDebugUserAsync();
        await AddUsersToSubjectsAsync(context);
        await AddResourcesToSubjectsAsync(context);
        await AddNceaItemsToResourcesAsync(context);
        await AddCambridgeItemsToResourcesAsync(context);
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

        // await AddSubjectsAsync(context, subjects);
    }


    internal static async Task AddSubjectsAsync(ApplicationDbContext context, SubjectJsonModel[] models)
    {
        foreach (var item in models)
        {
            var subject = (Subject)item;

            if (subject.Source == CurriculumSource.Ncea)
            {
                subject.NceaResource.AddRange(GetNceaResourcesForJsonModel(item));
            }
            else if (subject.Source == CurriculumSource.Cambridge)
            {
                //subject.CambridgeResource.AddRange(default!);
            }
        }

        await context.SaveChangesAsync();
    }

    internal static List<NceaResource> GetNceaResourcesForJsonModel(SubjectJsonModel model)
    {
        Debug.Assert(model.Curriculum.Source == CurriculumSource.Ncea);

        List<NceaResource> results = new(model.Resources.Length);

        foreach (var item in model.Resources)
        {
            Debug.Assert(item != null);
            Debug.Assert(item.Standard != null);

            var nceaResource = new NceaResource
            {
                AchievementStandard = item.Standard.AchievementStandard,
                Description = item.Standard.Description,
                AssessmentType = item.Standard.Type
            };

            results.Add(nceaResource);
        }

        return results;
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
                    item.NceaResource.Add(GenerateNceaStandard());
                }
            }
            else if (item.Source == CurriculumSource.Cambridge)
            {
                foreach (var _ in Enumerable.Range(0, Random.Shared.Next(1, 10)))
                {
                    item.CambridgeResource.Add(GenerateCambridgeResource());
                }
            }
        }

        context.UpdateRange(subjects);

        await context.SaveChangesAsync();
    }

    internal static async Task AddNceaItemsToResourcesAsync(ApplicationDbContext context)
    {
        var resources = await context.NceaResources.ToListAsync();

        AddItemsToResources(resources);

        context.NceaResources.UpdateRange(resources);

        await context.SaveChangesAsync();
    }

    internal static async Task AddCambridgeItemsToResourcesAsync(ApplicationDbContext context)
    {
        var resources = await context.CambridgeResource.ToListAsync();

        AddItemsToResources(resources);

        context.CambridgeResource.UpdateRange(resources);

        await context.SaveChangesAsync();
    }

    internal static void AddItemsToResources<TCustomResource>(List<TCustomResource> resources)
        where TCustomResource : CustomResource
    {
        foreach (var resource in resources)
        {
            foreach (var _ in Enumerable.Range(0, Random.Shared.Next(1, 10)))
            {
                var item = GenerateItem();

                if (resource.Items.Any(i => i.Year == item.Year))
                {
                    continue;
                }

                resource.Items.Add(item);
            }
        }
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
            Number = Random.Shared.Next(1, 6),
            Created = DateTimeOffset.UtcNow,
        };

        resource.SyncUpdated();

        return resource;
    }

    internal static Item GenerateItem()
    {
        var item = new Item()
        {
            Source = "/assets/dev/dev_test_pdf.pdf",
            Year = Random.Shared.Next(2012, DateTime.Now.Year),
            Created = DateTimeOffset.UtcNow
        };

        item.SyncUpdated();

        return item;
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

    internal async Task CreateDebugUserAsync()
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
}
