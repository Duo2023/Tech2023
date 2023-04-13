using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Tech2023.DAL.Models;

#nullable disable

/// <summary>
/// Represents the current privacy policy of the application
/// </summary>
public class PrivacyPolicy
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [JsonPropertyName("version")]
    public int Version { get; set; }

    [Required]
    [JsonPropertyName("content")]
    public string Content { get; set; }

    [Required]
    [JsonPropertyName("created")]
    public DateTimeOffset Created { get; set; }

    [Required]
    [JsonPropertyName("updated")]
    public DateTimeOffset Updated { get; set; }
}
