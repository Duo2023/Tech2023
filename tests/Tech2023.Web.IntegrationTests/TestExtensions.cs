namespace Tech2023.Web.IntegrationTests;

internal static class TestExtensions
{
    public static string CombinePaths(this string baseUrl, string resource)
    {
        return string.Concat((ReadOnlySpan<char>)baseUrl, "/", resource);
    }
}
