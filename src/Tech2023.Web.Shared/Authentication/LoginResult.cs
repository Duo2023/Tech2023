using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Tech2023.Web.Shared.Authentication;

/// <summary>
/// Returned to the API customer when the user signs in with their credentials
/// </summary>
public class LoginResult
{
    internal LoginResult(TokenObject? token, string? message)
    {
        Token = token;
        Message = message;
    }

    /// <summary>
    /// The JWT token used for authentication
    /// </summary>
    [MemberNotNullWhen(true, nameof(Success))]
    [JsonPropertyName("token")]
    public TokenObject? Token { get; internal set; }

    /// <summary>
    /// Boolean value to indicate whether the login is a success
    /// </summary>
    /// <remarks>
    /// If the property returns <see langword="true"/>, <see cref="Token"/> is guaranteed to be not <see langword="null"/>
    /// </remarks>
    [JsonPropertyName("success")]
    public bool Success { get; internal set; }

    /// <summary>The error message if included</summary>
    [JsonPropertyName("messsage")]
    public string? Message { get; internal set; }

    /// <summary>
    /// Returns a login result of 'Ok' as an object, this contains a token used as the JWT authentication token
    /// </summary>
    /// <param name="token">The token provided in the result</param>
    /// <returns>Valid 'Ok' response</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LoginResult Ok(TokenObject token)
    {
        ArgumentNullException.ThrowIfNull(token);

        return new LoginResult(token, default);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LoginResult Fail(string message)
    {
        ArgumentException.ThrowIfNullOrEmpty(message);

        return new LoginResult(default, message);
    }
}
