namespace Tech2023.Core;

public static class HtmlHexString
{
    /// <summary>
    /// Gets the color value as a hexadecimal string value
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe string GetHtmlHexString(uint value)
    {
        char* chars = stackalloc char[9]; // #FFFFFFFF max 

        *chars = '#'; // first character is '#' to use in css properties like { color: 

        _ = value.TryFormat(new Span<char>(chars + 1, 8), out int written, "x");

        return new string(chars, 0, written + 1);
    }
}
