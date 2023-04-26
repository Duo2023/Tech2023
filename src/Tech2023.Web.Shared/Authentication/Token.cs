using System.Text.Json.Serialization;

#nullable disable

namespace Tech2023.Web.Shared.Authentication;

/// <summary>
/// Represents a token, used for JWT refresh tokens
/// </summary>
public class Token
{
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; }

    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; }
}
