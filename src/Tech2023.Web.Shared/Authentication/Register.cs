using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Tech2023.DAL;

#nullable disable

namespace Tech2023.Web.Shared.Authentication;

/// <summary>
/// Register class
/// </summary>
public class Register
{
    [EmailAddress]
    [Display(Name = nameof(Email), Prompt = "Enter email")]
    [DataType(DataType.EmailAddress)] // Error message for this field is overriden in repective form using data-val-email
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter an E-mail address")]
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [MinLength(AuthConstants.MinPasswordLength, ErrorMessage = AuthConstants.PasswordLengthError)]
    [Display(Prompt = "Enter password")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a password")]
    [JsonPropertyName("password")]
    public string Password { get; set; }


    [DataType(DataType.Password)]
    [Display(Prompt = "Confirm password")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a password")]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    [MinLength(AuthConstants.MinPasswordLength, ErrorMessage = AuthConstants.PasswordLengthError)]
    [JsonPropertyName("confirmPassword")]
    public string ConfirmPassword { get; set; }
}
