using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Tech2023.Web.Shared;

internal static class WebEncoderHelpers
{
    internal const int StackallocThreshold = 128; // 128 bytes is the max amount of bytes before we go to heap basd allocations like ArrayPool

    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //public static bool TryDecodeFromFromBase64UrlUtf8String(ReadOnlySpan<char> input, [NotNullWhen(true)] out string? str)
    //{
    //    if (input.IsEmpty)
    //    {
    //        str = null;
    //        return false;
    //    }

    //    int requiredByteCount = Encoding.UTF8.Get

    //    str = "";
    //    return true;
    //}

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryEncodeToUtf8Base64Url(ReadOnlySpan<char> input, [NotNullWhen(true)] out string? str)
    {
        const int StackAllocThreshold = 256;

        if (input.IsEmpty)
        {
            str = null;
            return false;
        }

        int requiredByteCount = Encoding.UTF8.GetByteCount(input);

        Span<byte> buffer = requiredByteCount <= StackAllocThreshold ? stackalloc byte[requiredByteCount] : new byte[requiredByteCount];

        Encoding.UTF8.GetBytes(input, buffer);

        return TryEncodeInternal(buffer, out str);
    }

    /// <summary>
    /// Encodes <paramref name="input"/> using base64url encoding.
    /// </summary>
    /// <param name="input">The binary input to encode.</param>
    /// <returns>The base64url-encoded form of <paramref name="input"/>.</returns>
    [SkipLocalsInit]
    internal static bool TryEncodeInternal(ReadOnlySpan<byte> input, [NotNullWhen(true)] out string? output)
    {
        const int StackAllocThreshold = 128;

        if (input.IsEmpty)
        {
            output = null;
            return false;
        }

        int bufferSize = GetArraySizeRequiredToEncode(input.Length);

        char[]? bufferToReturnToPool = null;
        Span<char> buffer = bufferSize <= StackAllocThreshold
            ? stackalloc char[StackAllocThreshold]
            : bufferToReturnToPool = ArrayPool<char>.Shared.Rent(bufferSize);

        string? base64Url = null;

        if (TryEncodeInternal(input, buffer, out int written))
        {
            base64Url = new string(buffer[..written]);
        }
        else
        {
            output = null;
            return false;
        }

        if (bufferToReturnToPool != null)
        {
            ArrayPool<char>.Shared.Return(bufferToReturnToPool);
        }

        output = base64Url;
        return true;
    }

    public static int GetArraySizeRequiredToEncode(int count)
    {
        var numWholeOrPartialInputBlocks = checked(count + 2) / 3;
        return checked(numWholeOrPartialInputBlocks * 4);
    }

    private static bool TryEncodeInternal(ReadOnlySpan<byte> input, Span<char> output, out int written)
    {
        Debug.Assert(output.Length >= GetArraySizeRequiredToEncode(input.Length));

        if (input.IsEmpty)
        {
            written = 0;
            return false;
        }

        // Use base64url encoding with no padding characters. See RFC 4648, Sec. 5.

        if (!Convert.TryToBase64Chars(input, output, out int charsWritten))
        {
            written = 0;
            return false;
        }

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
                written = i;
                return true;
            }
        }

        written = charsWritten;
        return true;
    }
}
