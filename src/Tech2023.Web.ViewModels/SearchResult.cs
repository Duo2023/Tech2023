using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tech2023.Web.ViewModels;

#nullable disable

/// <summary>
/// A small struct used to contain search results to support the search infrastructure
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public readonly struct SearchResult
{
    internal SearchResult(string name, string url)
    {
        Name = name;
        Url = url;
    }

    /// <summary>
    /// The name of the search result to be displayed to the user
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The URL for this search result, used to navigate the user
    /// </summary>
    public string Url { get; }

    /// <summary>
    /// Creates a <see cref="SearchResult"/> using the supplied name and URI
    /// </summary>
    /// <param name="name">The name for the search result</param>
    /// <param name="url">The url to be used in the search result</param>
    /// <returns>Created <see cref="SearchResult"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SearchResult Create(string name, string url) => new(name, url);
}
