using Microsoft.EntityFrameworkCore;
using Tech2023.DAL.Models;

namespace Tech2023.DAL;

public static partial class Queries
{
    public static class Privacy
    {
        internal static readonly Func<ApplicationDbContext, Task<PrivacyPolicy>> _getCurrentPrivacyPolicy
            = EF.CompileAsyncQuery((ApplicationDbContext context) => context.PrivacyPolicies.OrderByDescending(p => p.Version).First());

        internal static readonly Func<ApplicationDbContext, int, Task<PrivacyPolicy?>> _getByVersion 
            = EF.CompileAsyncQuery((ApplicationDbContext context, int version) => context.PrivacyPolicies.FirstOrDefault(p => p.Version == version));


        public static Task<PrivacyPolicy> GetCurrentPrivacyPolicy(ApplicationDbContext context) => _getCurrentPrivacyPolicy(context);

        public static Task<PrivacyPolicy?> GetPrivacyByVersion(ApplicationDbContext context, int version) => _getByVersion(context, version);
    }
}
