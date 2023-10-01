﻿using Microsoft.EntityFrameworkCore;
using Tech2023.DAL.Models;

namespace Tech2023.DAL.Queries;

public static class Resources
{
    public static async Task<NceaResource?> FindNceaResourceByAchievementStandardAsync(ApplicationDbContext context, int achievementStandard)
    {
        return await context.NceaResources
            .Where(r => r.AchievementStandard == achievementStandard)
            .Include(r => r.Items)
            .FirstOrDefaultAsync();
    }

    public static async Task<CambridgeResource?> FindCambridgeResourceByIdentifersAsync(ApplicationDbContext context, int paperNumber, Season season, Variant variant)
    {
        return await context.CambridgeResource
            .Where(r => r.Number == paperNumber && r.Season == season && r.Variant == variant)
            .Include(r => r.Items)
            .FirstOrDefaultAsync();
    }
}
