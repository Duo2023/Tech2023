namespace Tech2023.DAL;

/// <summary>
/// Provides methods for parsing cambridge resource and formatting
/// </summary>
public static class Cambrige
{
    public static string GetString(int paperNumber, Season season, Variant variant)
    {
        return $"{paperNumber}_{season}_{variant}";
    }

    public static bool TryParseResource(ReadOnlySpan<char> str, out int paperNumber, Season season, Variant variant)
    {
        Unsafe.SkipInit(out paperNumber);
        Unsafe.SkipInit(out season);
        Unsafe.SkipInit(out variant);

        return false;
    }
}
