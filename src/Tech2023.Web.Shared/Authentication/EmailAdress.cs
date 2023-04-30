using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tech2023.Web.Shared.Authentication;

#nullable disable

/// <summary>
/// Represents an email address
/// </summary>
public class EmailAddress
{
    [EmailAddress]
    [Display(Name = nameof(Email), Prompt = "Enter email")]
    [DataType(DataType.EmailAddress)] // Error message for this field is overriden in repective form using data-val-email
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter an E-mail address")]
    [JsonPropertyName("email")]
    public string Email { get; set; }
}
