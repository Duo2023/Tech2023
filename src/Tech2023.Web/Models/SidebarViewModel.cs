using Tech2023.DAL.Models;

namespace Tech2023.Web.Models;
public class SidebarViewModel
{
    public required List<Subject> Subjects { get; set; } = new List<Subject>();
    public string? FilterArgs { get; set; }
    public Subject? SelectedSubject { get; set; }
}
