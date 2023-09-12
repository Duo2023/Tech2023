namespace Tech2023.DAL;

public interface IMetadata
{
    DateTimeOffset Created { get; set; }
    DateTimeOffset Updated { get; set; }
}

public static class MetadataExtensions
{
    public static void SyncUpdated(this IMetadata metadata)
    {
        ArgumentNullException.ThrowIfNull(metadata);

        metadata.Updated = metadata.Created;
    }
}
