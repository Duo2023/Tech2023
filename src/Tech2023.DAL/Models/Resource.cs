using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tech2023.DAL.Models;

#nullable disable

/// <summary>
/// A resource, this refers to 
/// </summary>
public class Resource : IMetadata
{
    /// <summary>
    /// The identifier of the resource, this is just used as a unique key for the database
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The revision year of the resource
    /// </summary>
    [Required]
    public int Year { get; set; }

    /// <summary>
    /// The source of the resource
    /// </summary>
    /// <remarks>
    /// Depending on what time of resource this refers to this refers to the static content where the paper is saved and should be considered always available
    /// </remarks>
    [Required]
    public string Source { get; set; }

    /// <summary>
    /// The last time the resource file was updated or changed in some way
    /// </summary>
    [Required]
    [DataType(DataType.DateTime)]
    [JsonPropertyName("updated")]
    public DateTimeOffset Updated { get; set; }

    /// <summary>
    /// The exact UTC time when the resource was created and added to the database
    /// </summary>
    [Required]
    [DataType(DataType.DateTime)]
    [JsonPropertyName("created")]
    public DateTimeOffset Created { get; set; }
}
