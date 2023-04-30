using System.Buffers;
using System.Security.Cryptography;

namespace Tech2023.Web.Shared.Authentication;

/// <summary>
/// Authentication helpers
/// </summary>
public static class AuthHelper
{
    /* Refresh Token:
     * In the current refresh token implementation, 
     * - 64 bytes are rented from an a shared ArrayPool<byte>
     * - The buffer is then filled with cryptographically secure bytes
     * - The bytes are converted to a Base64 string and returned
     * - The bytes are returned to the ArrayPool regardless of whether the method threw an exception
    */

    /// <summary>
    /// The number of bytes to be generated for a refresh token
    /// </summary>
    public const int RefreshTokenByteCount = 64;

    /// <summary>
    /// Generates a refresh token for the user
    /// </summary>
    /// <returns>A new refresh token</returns>
    public static string GenerateRefreshToken()
    {
        byte[] bytes = ArrayPool<byte>.Shared.Rent(RefreshTokenByteCount);

        try
        {
            RandomNumberGenerator.Fill(bytes);
            return Convert.ToBase64String(bytes);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(bytes);
        }
    }
}
