namespace Tech2023.Core;

/// <summary>
/// Represents the current environment configuration
/// </summary>
public static class EnvironmentConfiguration
{
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
