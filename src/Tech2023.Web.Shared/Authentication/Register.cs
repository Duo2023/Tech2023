using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Tech2023.DAL;

#nullable disable

namespace Tech2023.Web.Shared.Authentication;

/// <summary>
/// Registration input model used to get data to create the user
/// </summary>
public class Register
{
    /// <summary>
    /// The email address provided to use during registration, if the email already exists this will fail during creation
    /// </summary>
    [EmailAddress(ErrorMessage = "Invalid E-mail address")]
    [Display(Name = nameof(Email), Prompt = "Enter email")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter an E-mail address")]
    [JsonPropertyName("email")]
    public string Email { get; set; }

    /// <summary>
    /// The password for the user account provided by the user, has to be at least <see cref="AuthConstants.MinPasswordLength"/> and non <see langword="null"/>
    /// </summary>
    [DataType(DataType.Password)]
    [MinLength(AuthConstants.MinPasswordLength, ErrorMessage = AuthConstants.PasswordLengthError)]
    [Display(Prompt = "Enter password")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a password")]
    [JsonPropertyName("password")]
    public string Password { get; set; }

    /// <summary>
    /// A confirmation field, it must have the exact same contents as <see cref="Password"/> or validation fails
    /// </summary>
    [DataType(DataType.Password)]
    [Display(Prompt = "Confirm password")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a password")]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    [MinLength(AuthConstants.MinPasswordLength, ErrorMessage = AuthConstants.PasswordLengthError)]
    [JsonPropertyName("confirmPassword")]
    public string ConfirmPassword { get; set; }
}
