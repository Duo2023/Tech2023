using Microsoft.EntityFrameworkCore;
using Tech2023.DAL.Models;

namespace Tech2023.DAL.Queries;

/// <summary>
/// Queries related to the <see cref="CustomResource"/> abstract class and it's implementations
/// </summary>
public static class Resources
{
    internal static readonly Func<ApplicationDbContext, int, Task<NceaResource?>> _getByAchievementStandard 
        = EF.CompileAsyncQuery((ApplicationDbContext context, int standard) => context.NceaResources.Where(r => r.AchievementStandard == standard).Include(r => r.Items).FirstOrDefault());

    /// <summary>
    /// Finds an <see cref="NceaResource"/> by its achievement standard number which can be considered unique
    /// </summary>
    /// <param name="context">The context to use the query on</param>
    /// <param name="achievementStandard"></param>
    /// <returns><langword cref="null"/> if a resource with provided standard number doesn't exist, else the <see cref="NceaResource"/></returns>
    public static Task<NceaResource?> FindNceaResourceByAchievementStandardAsync(ApplicationDbContext context, int achievementStandard)
    {
        return _getByAchievementStandard(context, achievementStandard);
    }

    internal static readonly Func<ApplicationDbContext, int, Season, Variant, Task<CambridgeResource?>> _getByIdentifers
        = EF.CompileAsyncQuery((ApplicationDbContext context, int number, Season season, Variant variant) => context.CambridgeResource
            .Where(r => r.Number == number && r.Season == season && r.Variant == variant)
            .Include(r => r.Items)
            .FirstOrDefault());

    /// <summary>
    /// Finds a <see cref="CambridgeResource"/> by its various identifiers
    /// </summary>
    /// <param name="context">The context to operate on</param>
    /// <param name="paperNumber">The paper number</param>
    /// <param name="season">The season the paper belongs to</param>
    /// <param name="variant">The variant of the paper</param>
    /// <returns><langword cref="null"/> if a resource with provided identifiers doesn't exist, else the <see cref="CambridgeResource"/></returns>
    public static Task<CambridgeResource?> FindCambridgeResourceByIdentifersAsync(ApplicationDbContext context, int paperNumber, Season season, Variant variant)
    {
        return _getByIdentifers(context, paperNumber, season, variant);
    }
}
