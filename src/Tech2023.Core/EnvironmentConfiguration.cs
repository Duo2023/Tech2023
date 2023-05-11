namespace Tech2023.Core;

/// <summary>
/// Represents the current environment configuration
/// </summary>
public static class EnvironmentConfiguration
{
    /*
     * Do not change and mark these fields as const instead of static readonly, this is intentional
     * When used in razor pages the compiler treats a const as if (true) and flag an unreachable error
     * The compiler will optimize away the other branch of if (IsDebug) {} else {} anyway
     */

    /// <summary>
    /// Whether the executing process was built in Debug
    /// </summary>
    public static readonly bool IsDebug =
#if DEBUG
            true;
#else
            false;
#endif

    /// <summary>
    /// Whether the executing process was built in Release
    /// </summary>
    public static readonly bool IsRelease =
#if RELEASE
            true;
#else
            false;
#endif
}
