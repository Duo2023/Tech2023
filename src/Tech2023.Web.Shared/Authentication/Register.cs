using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

#nullable disable

namespace Tech2023.Web.Shared.Authentication;

/// <summary>
/// Register class
/// </summary>
public class Register
{
    [Required]
    [DataType(DataType.EmailAddress)]
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [JsonPropertyName("password")]
    public string Password { get; set; }

    [Required]
    [Compare(nameof(Password))]
    [DataType(DataType.Password)] 
    [JsonPropertyName("confirmPassword")]
    public string ConfirmPassword { get; set; }
}
