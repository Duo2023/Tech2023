using System.Diagnostics;

namespace Tech2023.DAL;

/// <summary>
/// Represents creation and update data about a record
/// </summary>
public interface IMetadata
{
    /// <summary>
    /// The time that the record was created in the database
    /// </summary>
    DateTimeOffset Created { get; set; }

    /// <summary>
    /// The last time that the record was updated in the database
    /// </summary>
    DateTimeOffset Updated { get; set; }
}

/// <summary>
/// Provides convience extension methods for a class type that implements <see cref="IMetadata"/>
/// </summary>
public static class MetadataExtensions
{
    /// <summary>
    /// Syncs the updated timestamp to the creation timestamp
    /// </summary>
    /// <param name="metadata">The metadata to pass in</param>
    public static void SetToCurrent(this IMetadata metadata)
    {
        Debug.Assert(metadata != null); // only check whether the record is null in debug builds

        metadata.Created = DateTimeOffset.UtcNow;
        metadata.Updated = metadata.Created;
    }

    /// <summary>
    /// Update the timestamp for the last time that a record with editing metadata with edited to the current time with standard UTC offset
    /// </summary>
    /// <param name="metadata">The type of metadata to pass in</param>
    public static void SyncLatest(this IMetadata metadata)
    {
        Debug.Assert(metadata != null);

        metadata.Updated = DateTimeOffset.UtcNow;
    }
}
