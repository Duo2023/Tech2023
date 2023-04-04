using System.Runtime.CompilerServices;

namespace Tech2023.Web.API.Extensions;

internal static class ConfigurationExtensions
{
    /// <summary>
    /// Gets a JWT option from the <see cref="IConfiguration"/> instance
    /// </summary>
    /// <param name="configuration">The configuration to use</param>
    /// <param name="key">The associated key in the <see cref="IConfiguration"/> instance</param>
    /// <returns>The value from the property, if it exists</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string? GetJwtOption(this IConfiguration configuration, string key)
    {
        return configuration.GetSection("Jwt")[key];
    }
}
