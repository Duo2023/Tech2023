namespace Tech2023.Web.ViewModels;

/// <summary>
/// Data used to for editing subjects
/// </summary>
public class SubjectEditModel
{
    /// <summary>
    /// Identifiers to add
    /// </summary>
    public List<Guid> Add { get; } = new();

    /// <summary>
    /// Identifiers to remove
    /// </summary>
    public List<Guid> Delete { get; } = new();
}
