using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

#nullable disable

namespace Tech2023.Web.Shared;

/// <summary>
/// Login model for the user to sign into the api
/// </summary>
public class Login
{
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    [Required(AllowEmptyStrings = false)]
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [Required(AllowEmptyStrings = false)]
    [JsonPropertyName("password")]
    public string Password { get; set; }
}
