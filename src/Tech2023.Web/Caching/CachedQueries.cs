using System.Diagnostics;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Tech2023.DAL;
using Tech2023.DAL.Queries;
using Tech2023.Web.ViewModels;

namespace Tech2023.Web.Caching;

public class CachedQueries
{
    /// <summary>
    /// Attempts to load all subjects from the cache if it exists else 
    /// </summary>
    public static async ValueTask<List<SubjectViewModel>> GetSubjectViewModelsFromCacheOrFetchAsync(IDbContextFactory<ApplicationDbContext> factory, IMemoryCache cache)
    {
        Debug.Assert(factory != null);
        Debug.Assert(cache != null);

        List<SubjectViewModel> subjects;

        if (cache.TryGetValue(CacheSlots.Subjects, out var data)) // fast path
        {
            if (data is List<SubjectViewModel> list)
            {
                subjects = list;
                return subjects;
            }
        }

        using (var context = await factory.CreateDbContextAsync())
        {
            subjects = await Subjects.GetAllSubjectViewModelsAsync(context);
        }

        cache.Set(CacheSlots.Subjects, subjects);

        return subjects;
    }
}
