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
    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [Required(AllowEmptyStrings = false)]
    [DataType(DataType.Password)]
    [MinLength(AuthConstants.MinPasswordLength)]
    [JsonPropertyName("password")]
    public string Password { get; set; }

    [Required(AllowEmptyStrings = false)]
    [Compare(nameof(Password))]
    [DataType(DataType.Password)]
    [MinLength(AuthConstants.MinPasswordLength)]
    [JsonPropertyName("confirmPassword")]
    public string ConfirmPassword { get; set; }
}
