using Tech2023.DAL.Models;

namespace Tech2023.Web.Models.Components;
  
public sealed class SidebarViewModel
{
    public required List<Subject> Subjects { get; set; } = new List<Subject>();

    public BrowsePapersViewModel? BrowseData { get; set; }
}
