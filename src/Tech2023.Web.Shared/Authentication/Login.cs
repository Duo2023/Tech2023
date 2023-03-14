using System.ComponentModel.DataAnnotations;

namespace Tech2023.Web.Shared.Authentication;

// https://www.trystanwilcock.com/2022/08/13/net-6-0-jwt-token-authentication-c-sharp-api-tutorial/

#nullable disable

/// <summary>
/// A login to send the web api and receive a <see cref="AuthResult"/> in response
/// </summary>
public class Login
{
    /// <summary>
    /// The email address to the target associated account
    /// </summary>
    [EmailAddress]
    [Required]
    public string Email { get; set; }

    /// <summary>
    /// The password for the target user account
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
