using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Tech2023.DAL.Models;

namespace Tech2023.DAL;

/// <summary>
/// Represents a subject that can any type of resource attached to it
/// </summary>
public class Subject
{
    /// <summary>
    /// The unique key of the subject, this field is not tied to the uniqueness and encodes no useful information about what the subject is apart from the subject
    /// </summary>
    [Key]
    [Required]
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Represents the name of the subject, 
    /// this field is not unique property but has some say on additional subjects being added whether they are NCEA or Cambridge subjects that are non unique
    /// </summary>
    /// <remarks>
    /// On creation of a new a subject if it occurs it will be checked with <see cref="StringComparison.OrdinalIgnoreCase"/>, case insensitive
    /// </remarks>
#nullable disable // disable for reference types because they are implicitly not null
    [Required(AllowEmptyStrings = false)]
    [MinLength(2)] // any subject is going to be at least about 2 characters
    [JsonPropertyName("name")]
    public string Name { get; set; }
#nullable restore

    /// <summary>
    /// The curriculum source of the subject, this affects paper retrieval and display formatting on the front end.
    /// </summary>
    [Required]
    [JsonPropertyName("curriculumSource")]
    public CurriculumSource Source { get; set; }

    /// <summary>
    /// Unsigned 4 byte integer used for the preffered display color if it is <see langword="null"/>
    /// </summary>
    [JsonPropertyName("displayColor")]
    public uint? DisplayColor { get; set; }
}
