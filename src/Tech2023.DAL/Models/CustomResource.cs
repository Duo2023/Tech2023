using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Tech2023.DAL.Models;

namespace Tech2023.DAL;

/// <summary>
/// A custom resource is a class that is meant to be inherited for each source for the common shared properties for less code duplication
/// </summary>
/// <remarks>
/// In an external database the SQL will show different tables and T-SQL statements
/// </remarks>
public abstract class CustomResource : IMetadata
{
    /// <summary>
    /// The identifier of the NCEA resource, this value is not related to the resource at all and it just provided a unique identifier to use in the database
    /// that can be created at random with a 2^128-1 amount of values to be generated from for uniqueness
    /// </summary>
    [Key]
    [JsonPropertyName("id")] //
    public Guid Id { get; set; }

    /// <summary>
    /// The last time the resource was updated in the database, 
    /// this is not the same time it may have been updated on a different website
    /// </summary>
    [Required]
    [DataType(DataType.DateTime)]
    [JsonPropertyName("updated")]
    public DateTimeOffset Updated { get; set; }

    /// <summary>
    /// When it was created, this means when it was created in our database and not the ncea version
    /// </summary>
    [Required]
    [DataType(DataType.DateTime)]
    [JsonPropertyName("created")]
    public DateTimeOffset Created { get; set; }

#nullable disable
    /// <summary>
    /// The resource items that belong to this resource
    /// </summary>
    [Required]
    [JsonPropertyName("resources")]
    public virtual List<Resource> Resources { get; } = new();
}
