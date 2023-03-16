using System.Text.Json.Serialization;

namespace Tech2023.Web.Shared.Authentication;

/// <summary>
/// Represents an API result response of the action of sending a <see cref="Login"/>
/// </summary>
public class AuthResult
{
    /// <summary>Cache this so we don't have to return a new instance each time and waste GC collects</summary>
    internal static readonly AuthResult _ok = new(true, Enumerable.Empty<string>());

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthResult"/>
    /// </summary>
    /// <param name="success">Whether the operation succeeded</param>
    /// <param name="errors">The enumerable of errors that occured if any</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal AuthResult(bool success, IEnumerable<string> errors)
    {
        Success = success;
        Errors = errors;
    }

    /// <summary>
    /// Returns a fail response with a enumerable of errors
    /// </summary>
    /// <param name="errors">The errors that occured during login</param>
    /// <returns>A <see cref="AuthResult"/> where the <see cref="Success"/> is specified as false</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static AuthResult Fail(IEnumerable<string> errors)
    {
        return new(false, errors);
    }

    /// <summary>
    /// Returns a successful response, the login has worked
    /// </summary>
    /// <returns>A successful login</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static AuthResult Ok() => _ok;

    /// <summary>
    /// Whether the login succeeded or not
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; internal set; }

#nullable disable

    /// <summary>
    /// Contains a collection of errors that the login has had
    /// </summary>
    /// <remarks>
    /// This property may not always be located in the api response
    /// </remarks>
    [JsonPropertyName("errors")]
    public IEnumerable<string> Errors { get; internal set; }
}
