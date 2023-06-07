using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Tech2023.Web.Shared;

internal static class WebEncoderHelpers
{
    internal const int StackallocThreshold = 128; // 128 bytes is the max amount of bytes before we go to heap basd allocations like ArrayPool

    private const char Base64PadCharacter = '=';
    private const char Base64Character62 = '+';
    private const char Base64Character63 = '/';
    private const char Base64UrlCharacter62 = '-';
    private const char Base64UrlCharacter63 = '_';  

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryDecodeFromBase64UrlEncoded(ReadOnlySpan<char> input, [NotNullWhen(true)] out string? output)
    {
        if (input.IsEmpty)
        {
            output = null;
            return false;
        }

        int requiredBytes = ((input.Length * 3) + 3) / 4;

        byte[]? bufferToReturnToPool = null;

        Span<byte> buffer = requiredBytes <= 256 ? stackalloc byte[requiredBytes] : (bufferToReturnToPool = ArrayPool<byte>.Shared.Rent(requiredBytes));

        if (!TryDecodeIntoBuffer(input, buffer, out int written))
        {
            FreeBuffer(bufferToReturnToPool);
            output = null;
            return false;
        }

        output = Encoding.UTF8.GetString(buffer[..written]);

        FreeBuffer(bufferToReturnToPool);

        return true;
    }

    internal static void FreeBuffer<T>(T[]? array)
    {
        if (array is not null)
        {
            ArrayPool<T>.Shared.Return(array);
        }
    }


    internal static unsafe bool TryDecodeIntoBuffer(ReadOnlySpan<char> input, Span<byte> bytes, out int written)
    {
        int mod = input.Length % 4;

        if (mod == 1)
        {
            written = 0;
            return false;
        }

        bool needReplace = false;
        int decodedLength = input.Length + (4 - mod) % 4;

        for (int i = 0; (uint)i < (uint)input.Length; i++)
        {
            if (input[i] == Base64Character62 || input[i] == Base64Character63)
            {
                needReplace = true;
                break;
            }
        }

        if (needReplace)
        {
            char[]? bufferToReturnToPool = null;
            Span<char> dest = decodedLength <= StackallocThreshold ? stackalloc char[StackallocThreshold] : (bufferToReturnToPool = ArrayPool<char>.Shared.Rent(decodedLength));

            dest = dest[..decodedLength];

            int i = 0;

            for (; i < input.Length; i++)
            {
                if (input[i] == Base64Character62)
                {
                    dest[i] = Base64Character62;
                }
                else if (input[i] == Base64Character63)
                {
                    dest[i] = Base64Character63;
                }
                else
                {
                    dest[i] = input[i];
                }
            }

            for (; i < decodedLength; i++)
            {
                dest[i] = Base64PadCharacter;
            }
            

            bool result = Convert.TryFromBase64Chars(dest, bytes, out written);

            if (bufferToReturnToPool != null)
            {
                ArrayPool<char>.Shared.Return(bufferToReturnToPool);
            }

            return result;
        }
        else
        {
            if (decodedLength == input.Length)
            {
                return Convert.TryFromBase64Chars(input, bytes, out written);
            }

            char[]? bufferToReturnToPool = null;

            Span<char> decoded = decodedLength <= StackallocThreshold ? stackalloc char[StackallocThreshold] : (bufferToReturnToPool = ArrayPool<char>.Shared.Rent(decodedLength));

            decoded = decoded[..decodedLength];

            input.CopyTo(decoded);

            bool result = Convert.TryFromBase64Chars(decoded, bytes, out written);

            if (bufferToReturnToPool != null)
            {
                ArrayPool<char>.Shared.Return(bufferToReturnToPool);
            }

            return result;
        }
    }

    #region Encode

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
        byte[]? bufferToReturnToPool = null;

        Span<byte> buffer = requiredByteCount <= StackAllocThreshold 
            ? stackalloc byte[requiredByteCount] 
            : bufferToReturnToPool = ArrayPool<byte>.Shared.Rent(requiredByteCount);

        buffer = buffer[..requiredByteCount];

        Encoding.UTF8.GetBytes(input, buffer);

        bool result = TryEncodeInternal(buffer, out str);

        if (bufferToReturnToPool != null)
        {
            ArrayPool<byte>.Shared.Return(bufferToReturnToPool);
        }

        return result;
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
    #endregion // end Encode
}
