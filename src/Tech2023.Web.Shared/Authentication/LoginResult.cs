using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Tech2023.Web.Shared.Authentication;

/// <summary>
/// Returned to the API customer when the user signs in with their credentials
/// </summary>
public class LoginResult
{
    /// <summary>
    /// Initializes a new instance 
    /// </summary>
    /// <param name="token"></param>
    /// <param name="errors"></param>
    internal LoginResult(string? token, IEnumerable<string> errors)
    {
        Success = token is not null;
        Token = token;
        Errors = errors;
    }

    /// <summary>
    /// The JWT token used for authentication
    /// </summary>
    [MemberNotNullWhen(true, nameof(Success))]
    [JsonPropertyName("token")]
    public string? Token { get; internal set; }

    /// <summary>
    /// Boolean value to indicate whether the login is a success
    /// </summary>
    /// <remarks>
    /// If the property returns <see langword="true"/>, <see cref="Token"/> is guaranteed to be not <see langword="null"/>
    /// </remarks>
    [JsonPropertyName("success")]
    public bool Success { get; internal set; }

    /// <summary>
    /// The collection of errors related with the sign-in, if <see cref="Success"/> property returns <see langword="true"/> this collection will be empty
    /// </summary>
    [JsonPropertyName("errors")]
    public IEnumerable<string> Errors { get; set; }

    /// <summary>
    /// Returns a login result of 'Ok' as an object, this contains a token used as the JWT authentication token
    /// </summary>
    /// <param name="token">The token provided in the result</param>
    /// <returns>Valid 'Ok' response</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LoginResult Ok(string token)
    {
        ArgumentException.ThrowIfNullOrEmpty(token);

        return new LoginResult(token, Enumerable.Empty<string>());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LoginResult Fail(IEnumerable<string> errors)
    {
        if (!errors.Any())
        {
            throw new ArgumentException("Sequence cannot contain no elements as this is a fail case");
        }

        return new LoginResult(null, errors);
    }
}
