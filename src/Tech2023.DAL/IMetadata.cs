using System.Diagnostics;

namespace Tech2023.DAL;

public interface IMetadata
{
    DateTimeOffset Created { get; set; }
    DateTimeOffset Updated { get; set; }
}

public static class MetadataExtensions
{
    /// <summary>
    /// Syncs the updated timestamp to the creation timestamp
    /// </summary>
    /// <param name="metadata">The metadata to pass in</param>
    public static void SyncUpdated(this IMetadata metadata)
    {
        Debug.Assert(metadata != null); // only check whether the record is null in debug builds

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
