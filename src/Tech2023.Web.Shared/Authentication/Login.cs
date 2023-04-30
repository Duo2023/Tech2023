using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Tech2023.DAL;

#nullable disable

namespace Tech2023.Web.Shared;

/// <summary>
/// Login model for the user to sign into the api
/// </summary>
public class Login
{
    [EmailAddress]
    [Display(Name = nameof(Email), Prompt = "Enter email")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Invalid E-mail address")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter an E-mail address")]
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [MinLength(AuthConstants.MinPasswordLength, ErrorMessage = AuthConstants.PasswordLengthError)]
    [Display(Prompt = "Enter password")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a password")]
    [JsonPropertyName("password")]
    public string Password { get; set; }
}
