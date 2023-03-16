using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Tech2023.Web.Shared.Authentication;

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

    [MemberNotNullWhen(true, nameof(Success))]
    public string? Token { get; internal set; }

    [JsonPropertyName("success")]
    public bool Success { get; internal set; }

    [JsonPropertyName("errors")]
    public IEnumerable<string> Errors { get; set; }
}
