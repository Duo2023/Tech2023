using Tech2023.DAL.Models;
using Tech2023.Web.ViewModels;

namespace Tech2023.Web.Models.Components;
  
public sealed class SidebarViewModel
{
    public required List<SubjectViewModel> SavedSubjects { get; set; }
    public required List<SubjectViewModel> AllSubjects { get; set; }

    public BrowsePapersViewModel? BrowseData { get; set; }
}
