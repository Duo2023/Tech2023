using System.Text.Json.Serialization;

namespace Tech2023.Web.Shared.Authentication;

#nullable disable

/// <summary>
/// A token object that is included when you login
/// </summary>
public class TokenObject
{
    /// <summary>
    /// The actual JWT token used for authentication
    /// </summary>
    [JsonPropertyName("token")]
    public string Token { get; set; }

    /// <summary>
    /// The expiration of the token
    /// </summary>
    [JsonPropertyName("expiration")]
    public DateTime Expiration { get; set; }
}
