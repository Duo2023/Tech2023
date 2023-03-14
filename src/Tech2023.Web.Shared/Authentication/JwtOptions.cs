namespace Tech2023.Web.Shared.Authentication;

#nullable disable

public class JwtOptions
{
    /// <summary>
    /// The secret signing key
    /// </summary>
    public string Secret { get; set; }

    /// <summary>
    /// The issuer of the key
    /// </summary>
    public string ValidIssuer { get; set; }

    /// <summary>
    /// The audience of the key
    /// </summary>
    public string ValidAudience { get; set; }

    /// <summary>
    /// The time the JWT expires in
    /// </summary>
    public uint ExpiryInMinutes { get; set; }
}
