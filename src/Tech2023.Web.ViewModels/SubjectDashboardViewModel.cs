using Tech2023.DAL.Models;

namespace Tech2023.Web.ViewModels;

#nullable disable

/// <summary>
/// View model used for supporting the dashboard subjects page
/// </summary>
public class SubjectDashboardViewModel
{
    /// <summary>
    /// The saved subjects of a user, if any
    /// </summary>
    public List<SubjectViewModel> SavedSubjects { get; set; }

    /// <summary>
    /// The recent resources the user has accessed
    /// </summary>
    public List<Item> RecentResources { get; set; }
}
