using Microsoft.EntityFrameworkCore;
using Tech2023.DAL.Models;

namespace Tech2023.DAL;

public static partial class Queries
{
    /// <summary>
    /// Static requires relating to privacy models
    /// </summary>
    public static class Privacy
    {
        internal static readonly Func<ApplicationDbContext, Task<PrivacyPolicy>> _getCurrentPrivacyPolicy
            = EF.CompileAsyncQuery((ApplicationDbContext context) => context.PrivacyPolicies.OrderByDescending(p => p.Version).First());

        internal static readonly Func<ApplicationDbContext, int, Task<PrivacyPolicy?>> _getByVersion 
            = EF.CompileAsyncQuery((ApplicationDbContext context, int version) => context.PrivacyPolicies.FirstOrDefault(p => p.Version == version));


        /// <summary>
        /// Gets the latest privacy policy
        /// </summary>
        /// <param name="context">The context to retrieve the policy from</param>
        /// <returns>Non-null latest privacy policy</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task<PrivacyPolicy> GetPolicyAsync(ApplicationDbContext context) => _getCurrentPrivacyPolicy(context);

        /// <summary>
        /// Gets the privacy policy by version if available
        /// </summary>
        /// <param name="context">The context to retrieve the policy from</param>
        /// <param name="version">The version of the privacy policy</param>
        /// <returns>A possibly null Privacy Policy if the version doesn't exist</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task<PrivacyPolicy?> GetByVersionAsync(ApplicationDbContext context, int version) => _getByVersion(context, version);
    }
}
