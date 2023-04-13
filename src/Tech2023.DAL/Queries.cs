using Microsoft.EntityFrameworkCore;

using Tech2023.DAL.Models;

namespace Tech2023.DAL;

public static class Queries
{
    internal static readonly Func<ApplicationDbContext, Task<PrivacyPolicy>> _getCurrentPrivacyPolicy
        = EF.CompileAsyncQuery((ApplicationDbContext context) => context.PrivacyPolicies.OrderByDescending(p => p.Version).First());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<PrivacyPolicy> GetCurrentPrivacyPolicy(ApplicationDbContext context) => _getCurrentPrivacyPolicy(context);
}
