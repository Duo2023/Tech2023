namespace Tech2023.Core;

/// <summary>
/// Class used to generate CSS Hex Color representation
/// </summary>
public static class HtmlHexString
{
    /// <summary>One byte in hex is 2 digits, 2 digits * sizeof(uint) = 8. +1 for # character at the start</summary>
    internal const int BufferSize = 9;

    /// <summary>
    /// Gets the color value as a hexadecimal string value
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe string GetHtmlHexString(uint value)
    {
        char* chars = stackalloc char[BufferSize];

        *chars = '#'; // set the first element in memory no matter what to #

        _ = value.TryFormat(new Span<char>(chars + 1, 8), out int written, "x"); // create a span from the 2nd memory element and let the standard library do the formatting

        return new string(chars, 0, written + 1); // create a new string from the stack buffer only including characters that have been written +1 for #
    }
}
