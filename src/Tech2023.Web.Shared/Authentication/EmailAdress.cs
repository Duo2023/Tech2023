using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tech2023.Web.Shared.Authentication;

#nullable disable

/// <summary>
/// Represents an email address, many input models use this class as some only take just emails
/// </summary>
public class EmailAddress
{
    /// <summary>
    /// The email address of the user
    /// </summary>
    [EmailAddress]
    [Display(Name = nameof(Email), Prompt = "Enter email")]
    [DataType(DataType.EmailAddress)] // Error message for this field is overriden in repective form using data-val-email
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter an E-mail address")]
    [JsonPropertyName("email")]
    public string Email { get; set; }
}
