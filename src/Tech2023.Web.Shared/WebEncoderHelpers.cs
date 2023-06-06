using System.Buffers;
using System.Diagnostics;
using System.Text;

namespace Tech2023.Web.Shared;

internal static class WebEncoderHelpers
{
    internal const int StackallocThreshold = 128; // 128 bytes is the max amount of bytes before we go to heap basd allocations like ArrayPool

    public static string EncodeAsUTF8ToBase64Url(string input)
    {
        return Base64UrlEncode(Encoding.UTF8.GetBytes(input));
    }

    /// <summary>
    /// Encodes <paramref name="input"/> using base64url encoding.
    /// </summary>
    /// <param name="input">The binary input to encode.</param>
    /// <returns>The base64url-encoded form of <paramref name="input"/>.</returns>
    [SkipLocalsInit]
    internal static string Base64UrlEncode(ReadOnlySpan<byte> input)
    {
        const int StackAllocThreshold = 128;

        if (input.IsEmpty)
        {
            return string.Empty;
        }

        int bufferSize = GetArraySizeRequiredToEncode(input.Length);

        char[]? bufferToReturnToPool = null;
        Span<char> buffer = bufferSize <= StackAllocThreshold
            ? stackalloc char[StackAllocThreshold]
            : bufferToReturnToPool = ArrayPool<char>.Shared.Rent(bufferSize);

        var numBase64Chars = Base64UrlEncode(input, buffer);
        var base64Url = new string(buffer[..numBase64Chars]);

        if (bufferToReturnToPool != null)
        {
            ArrayPool<char>.Shared.Return(bufferToReturnToPool);
        }

        return base64Url;
    }

    public static int GetArraySizeRequiredToEncode(int count)
    {
        var numWholeOrPartialInputBlocks = checked(count + 2) / 3;
        return checked(numWholeOrPartialInputBlocks * 4);
    }

    private static int Base64UrlEncode(ReadOnlySpan<byte> input, Span<char> output)
    {
        Debug.Assert(output.Length >= GetArraySizeRequiredToEncode(input.Length));

        if (input.IsEmpty)
        {
            return 0;
        }

        // Use base64url encoding with no padding characters. See RFC 4648, Sec. 5.

        Convert.TryToBase64Chars(input, output, out int charsWritten);

        // Fix up '+' -> '-' and '/' -> '_'. Drop padding characters.
        for (var i = 0; i < charsWritten; i++)
        {
            var ch = output[i];
            if (ch == '+')
            {
                output[i] = '-';
            }
            else if (ch == '/')
            {
                output[i] = '_';
            }
            else if (ch == '=')
            {
                // We've reached a padding character; truncate the remainder.
                return i;
            }
        }

        return charsWritten;
    }
}
