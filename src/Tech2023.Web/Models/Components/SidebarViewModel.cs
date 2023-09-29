using Tech2023.DAL.Models;
using Tech2023.Web.ViewModels;

namespace Tech2023.Web.Models.Components;
  
public sealed class SidebarViewModel
{
    public required List<SubjectViewModel> Subjects { get; set; } = new List<SubjectViewModel>();

    public BrowsePapersViewModel? BrowseData { get; set; }
}
