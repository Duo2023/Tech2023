using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Tech2023.Core;

/// <summary>
/// Set of static methods for working and manipulating the <see cref="string"/> type
/// </summary>
public static class StringHelpers
{
    internal const int StackAllocThreshold = 256; // 512 bytes is a reasonable limit

    // TODO: Work needed on performance front, string on the smaller side allocate far less but the performance is slightly slower than than the Naiive version used

    /// <summary>
    /// Convert's the string to title case. Acryonyms are not supported so things like ID, will be converted to Id and organizational acronyms will not be converted
    /// </summary>
    /// <param name="str"></param>
    /// <returns>The converted title case variant</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToTitleCase(this string str)
    {
        // early return when the input is an empty string
        if (string.IsNullOrEmpty(str))
        {
            return string.Empty;
        }

        // if the buffer is equal or less than the threshold allocate on the stack else use arraypool version
        var result = str.Length <= StackAllocThreshold ?
            new ValueStringBuilder(stackalloc char[StackAllocThreshold])
            : new ValueStringBuilder(str.Length);

        bool capitalize = true;

        for (int i = 0; i < str.Length; i++)
        {
            char character = str[i];

            character = char.ToLower(character);

            if (char.IsWhiteSpace(character))
            {
                result.Append(character);
                capitalize = true;
            }
            else if (capitalize)
            {
                result.Append(char.ToUpper(character));
                capitalize = false;
            }
            else
            {
                result.Append(character);
            }
        }

        return result.ToString(); // ToString() calls dispose for us automatically
    }

    // from dotnet runtime formatting
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CountDigits(uint value)
    {
        // Algorithm based on https://lemire.me/blog/2021/06/03/computing-the-number-of-digits-of-an-integer-even-faster.
        ReadOnlySpan<long> table = new long[]
        {
                4294967296,
                8589934582,
                8589934582,
                8589934582,
                12884901788,
                12884901788,
                12884901788,
                17179868184,
                17179868184,
                17179868184,
                21474826480,
                21474826480,
                21474826480,
                21474826480,
                25769703776,
                25769703776,
                25769703776,
                30063771072,
                30063771072,
                30063771072,
                34349738368,
                34349738368,
                34349738368,
                34349738368,
                38554705664,
                38554705664,
                38554705664,
                41949672960,
                41949672960,
                41949672960,
                42949672960,
                42949672960,
        };
        Debug.Assert(table.Length == 32, "Every result of uint.Log2(value) needs a long entry in the table.");

        long tableValue = Unsafe.Add(ref MemoryMarshal.GetReference(table), uint.Log2(value));
        return (int)((value + tableValue) >> 32);
    }
}
