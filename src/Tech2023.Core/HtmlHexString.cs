using System.Diagnostics;
using System.Numerics;

namespace Tech2023.Core;

/// <summary>
/// Class used to generate CSS Hex Color representation
/// </summary>
public static class HtmlHexString
{
    internal const int HexBase = 'W';

    /// <summary>
    /// Gets the color value as a hexadecimal string value
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe string GetHtmlHexString(uint value)
    {
        int length = CountLength(value);

        char* chars = stackalloc char[length];

        *chars = '#'; // set the first element in memory no matter what to #

        int digits = -1;

        char* p = Int32ToHexChars(chars + length, value, digits);

        Debug.Assert(p == chars + 1);

        return new string(chars, 0, length); // create a new string from the stack buffer only including characters that have been written +1 for #
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe char* Int32ToHexChars(char* buffer, uint value, int digits)
    {
        while (--digits >= 0 || value != 0)
        {
            byte digit = (byte)(value & 0xF);
            *(--buffer) = (char)(digit + (digit < 10 ? (byte)'0' : HexBase));
            value >>= 4;
        }
        return buffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int CountLength(uint value)
    {
        // The number of hex digits is log16(value) + 1, or log2(value) / 4 + 1
        return (BitOperations.Log2(value) >> 2) + 2;
    }
}
