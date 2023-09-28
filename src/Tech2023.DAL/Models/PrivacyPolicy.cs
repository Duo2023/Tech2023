using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Tech2023.DAL.Models;

#nullable disable

/// <summary>
/// Represents the current privacy policy of the application
/// </summary>
public class PrivacyPolicy : IMetadata
{
    /// <summary>
    /// The version of the privacy policy, this is also the database key
    /// </summary>
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [JsonPropertyName("version")]
    public int Version { get; set; }

    /// <summary>
    /// The string content of the privacy policy
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    [JsonPropertyName("content")]
    public string Content { get; set; }

    /// <summary>
    /// The <see cref="DateTimeOffset"/> of when it was created, this will be <see cref="DateTimeOffset.UtcNow"/> for sync
    /// </summary>
    [Required]
    [DataType(DataType.DateTime)]
    [JsonPropertyName("created")]
    public DateTimeOffset Created { get; set; }

    /// <summary>
    /// The <see cref="DateTimeOffset"/> of when it was last updated, this will be <see cref="DateTimeOffset.UtcNow"/> for sync
    /// </summary>
    [Required]
    [DataType(DataType.DateTime)]
    [JsonPropertyName("updated")]
    public DateTimeOffset Updated { get; set; }
}
