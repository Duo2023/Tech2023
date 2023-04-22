using System.Buffers;
using System.Security.Cryptography;

namespace Tech2023.Web.Shared.Authentication;

public static class AuthHelper
{
    public static string GenerateRefreshToken()
    {
        byte[] bytes = ArrayPool<byte>.Shared.Rent(64);

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
