namespace Tech2023.Core;

/// <summary>
/// Represents the current environment configuration
/// </summary>
public static class EnvironmentConfiguration
{
    /// <summary>
    /// Whether the executing process was built in Debug
    /// </summary>
    public const bool IsDebug =
#if DEBUG
            true;
#else
            false;
#endif

    /// <summary>
    /// Whether the executing process was built in Release
    /// </summary>
    public const bool IsRelease =
#if RELEASE
            true;
#else
            false;
#endif
}
