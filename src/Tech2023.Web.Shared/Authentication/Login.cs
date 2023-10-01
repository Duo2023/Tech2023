using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Tech2023.DAL.Identity;

#nullable disable

namespace Tech2023.Web.Shared.Authentication;

/// <summary>
/// Login model for the user to sign into the api
/// </summary>
public class Login
{
    /// <summary>
    /// The login email address
    /// </summary>
    [EmailAddress(ErrorMessage = "Invalid E-mail address")]
    [Display(Name = nameof(Email), Prompt = "Enter email")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter an E-mail address")]
    [JsonPropertyName("email")]
    public string Email { get; set; }

    /// <summary>
    /// The password of the user account, if the password is not the same in the database it fails
    /// </summary>
    [DataType(DataType.Password)]
    [Display(Prompt = "Enter password")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a password")]
    [MinLength(AuthConstants.MinPasswordLength, ErrorMessage = AuthConstants.PasswordLengthError)]
    [JsonPropertyName("password")]
    public string Password { get; set; }
}
