using System.Diagnostics;
using System.Text.Json;
using Tech2023.DAL.Models;
using Tech2023.DAL;
using Tech2023.Web.Initialization.Json.Models;
using Tech2023.Web.Initialization.Json;
using Microsoft.EntityFrameworkCore;
using Tech2023.DAL.Queries;
using Tech2023.Web.Initialization.Generators;

namespace Tech2023.Web.Initialization;

// this file is only included when the application is built without optimizations in debug

/// <inheritdoc/>
internal partial class Initializer
{
    internal readonly IGenerator<CambridgeResource> _cambridgeResourceGenerator = new CambridgeResourceGenerator();
    internal readonly IGenerator<NceaResource> _nceaResourceGenerator = new NceaResourceGenerator();
    internal readonly IGenerator<Item> _itemGenerator = new ItemGenerator();

    internal async Task CreateDebuggingDataAsync(ApplicationDbContext context)
    {
        await CreateUserAsync("sudo@sudo.com", "sudoUser555!", admin: true);

        await CreateSubjectsAsync(context);
        await RandomizeUserSubjects(context);
        await AddResourcesToSubjectsAsync(context);

        await GenerateForResourcesSet(context.CambridgeResource);
        await GenerateForResourcesSet(context.NceaResources);

        await context.SaveChangesAsync();
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

    internal static void AddResourceForCollection<TCustomResource>(List<TCustomResource> resources, IGenerator<TCustomResource> generator) where TCustomResource : CustomResource
    {
        foreach (var _ in Enumerable.Range(0, Random.Shared.Next(1, 10)))
        {
            resources.Add(generator.Generate());
        }
    }

    internal async Task AddResourcesToSubjectsAsync(ApplicationDbContext context)
    {
        var subjects = await context.Subjects.ToListAsync();

        foreach (var item in subjects)
        {
            if (item.Source == CurriculumSource.Ncea)
            {
                AddResourceForCollection(item.NceaResource, _nceaResourceGenerator);
            }
            else if (item.Source == CurriculumSource.Cambridge)
            {
                AddResourceForCollection(item.CambridgeResource, _cambridgeResourceGenerator);
            }
        }

        context.UpdateRange(subjects);

        await context.SaveChangesAsync();
    }

    internal async Task GenerateForResourcesSet<TCustomResource>(DbSet<TCustomResource> set) where TCustomResource : CustomResource
    {
        var resources = await set.ToListAsync();

        RandomizeAndGenerateItemsForResources(resources);

        set.UpdateRange(resources);
    }


    internal void RandomizeAndGenerateItemsForResources<TCustomResource>(List<TCustomResource> resources)
        where TCustomResource : CustomResource
    {
        foreach (var resource in resources)
        {
            foreach (var _ in Enumerable.Range(0, Random.Shared.Next(1, 10)))
            {
                var item = _itemGenerator.Generate();

                if (resource.Items.Any(i => i.Year == item.Year))
                {
                    continue;
                }

                resource.Items.Add(item);
            }
        }
    }

    internal static async Task RandomizeUserSubjects(ApplicationDbContext context)
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
}
