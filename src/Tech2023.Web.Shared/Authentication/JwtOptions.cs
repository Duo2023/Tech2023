using System.Text.Json.Serialization;

namespace Tech2023.Web.Shared.Authentication;

#nullable disable

/// <summary>
/// Configures the Json Web token settings
/// </summary>
public class JwtOptions
{
    /// <summary>
    /// The secret signing key
    /// </summary>
    [JsonPropertyName("secret")]
    public string Secret { get; set; }

    /// <summary>
    /// The issuer of the key
    /// </summary>
    [JsonPropertyName("validIssuer")]
    public string ValidIssuer { get; set; }

    /// <summary>
    /// The audience of the key
    /// </summary>
    [JsonPropertyName("validAudience")]
    public string ValidAudience { get; set; }

    /// <summary>
    /// The time the JWT expires in
    /// </summary>
    [JsonPropertyName("expiryInMinutes")]
    public uint ExpiryInMinutes { get; set; }

    [JsonPropertyName("refreshTokenValidityInDays")]
    public uint RefreshTokenValid { get; set; }
}
