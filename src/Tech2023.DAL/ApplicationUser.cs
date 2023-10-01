using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Identity;

using Tech2023.DAL.Models;

namespace Tech2023.DAL;

/// <summary>
/// Represents a user in the application, this used with <see cref="ApplicationDbContext"/> and during account creations.
/// </summary>
public class ApplicationUser : IdentityUser<Guid>, IMetadata
{
    /// <inheritdoc/>
    [JsonPropertyName("created")]
    public DateTimeOffset Created { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("updated")]
    public DateTimeOffset Updated { get; set; }

    /// <summary>
    /// A list of the users saved subjects, this is a one-to-many relationship with subjects
    /// </summary>
    public virtual List<Subject> SavedSubjects { get; set; } = new();
}
