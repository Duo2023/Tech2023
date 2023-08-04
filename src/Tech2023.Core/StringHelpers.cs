using System.Diagnostics.Contracts;
using System.Globalization;

namespace Tech2023.Core;

public static class StringHelpers
{
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToTitleCase(this string str)
    {
        // TODO: More efficient implementation because this allocates an intemediatry string that gets thrown away
        return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(str.ToLower());
    }
}
