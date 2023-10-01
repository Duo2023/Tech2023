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
}
