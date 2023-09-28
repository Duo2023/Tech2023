using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tech2023.Web.ViewModels;

#nullable disable

[StructLayout(LayoutKind.Sequential)]
public readonly struct SearchResult
{
    internal SearchResult(string name, string url)
    {
        Name = name;
        Url = url;
    }

    public string Name { get; }

    public string Url { get; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SearchResult Create(string name, string url) => new(name, url);
}
