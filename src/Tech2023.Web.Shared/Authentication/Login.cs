using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Tech2023.DAL;

#nullable disable

namespace Tech2023.Web.Shared.Authentication;

/// <summary>
/// Login model for the user to sign into the api
/// </summary>
public class Login
{
    [EmailAddress]
    [Display(Name = nameof(Email), Prompt = "Enter email")]
    [DataType(DataType.EmailAddress)] // Error message for this field is overriden in repective form using data-val-email
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter an E-mail address")]
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [Display(Prompt = "Enter password")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a password")]
    [MinLength(AuthConstants.MinPasswordLength, ErrorMessage = AuthConstants.PasswordLengthError)]
    [JsonPropertyName("password")]
    public string Password { get; set; }
}
