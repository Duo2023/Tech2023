using System.Diagnostics;
using System.Numerics;

namespace Tech2023.Core;

/// <summary>
/// Class used to generate CSS Hex Color representation
/// </summary>
public static class HtmlHexString
{
    /// <summary>
    /// Gets the color value as a hexadecimal html string value, eg. #aaffbbcc
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe string GetHtmlHexString(uint value)
    {
        int length = CountLength(value);

        Debug.Assert(length < 10);

        char* chars = stackalloc char[length];

        *chars = '#'; // set the first element in memory no matter what to #

        int digits = -1;

        char* p = Int32ToHexChars(chars + length, value, digits);

        Debug.Assert(p == chars + 1);

        return new string(chars, 0, length); // create a new string from the stack buffer only including characters that have been written +1 for #
    }

    internal const int HexBase = 'W';

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe char* Int32ToHexChars(char* buffer, uint value, int digits)
    {
        while (--digits >= 0 || value != 0) // loop over the hex digits
        {
            byte digit = (byte)(value & 0xF); // get the digit

            // If the digit is in the range 0-9 add it to the ASCII value of 0 and every value will fit into the ASCII numeric digit range between 48 to 57
            // Else if the digit is 10 >= add it to the base 'W' (87) which we can then add 10-15 onto to fit into the abcdf (97-102) range
            *--buffer = (char)(digit + (digit < 10 ? (byte)'0' : HexBase));

            value >>= 4; // bitshift for the next number
        }

        return buffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int CountLength(uint value)
    {
        // The number of hex digits is log16(value) + 1, or log2(value) / 4 + 1
        // Add +2 for the # character to be included
        return (BitOperations.Log2(value) >> 2) + 2;
    }

    /// <summary>
    /// Generate a random unsigned 32-bit integer
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint NextColor()
    {
        return (uint)Random.Shared.Next(-int.MaxValue, int.MaxValue);
    }

}
