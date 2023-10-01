namespace Tech2023.Web.ViewModels;

/// <summary>
/// View data for the sidebar
/// </summary>
public sealed class SidebarViewModel
{
    /// <summary>
    /// All of the users saved subjects
    /// </summary>
    public required List<SubjectViewModel> SavedSubjects { get; set; }

    /// <summary>
    /// All of the available subjects
    /// </summary>
    public required List<SubjectViewModel> AllSubjects { get; set; }

    /// <summary>
    /// Browsing data for the sidebar
    /// </summary>
    public BrowsePapersViewModel? BrowseData { get; set; }
}
