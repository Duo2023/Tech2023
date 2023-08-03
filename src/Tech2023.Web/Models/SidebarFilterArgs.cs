using Tech2023.DAL.Models;

namespace Tech2023.Web.Models;
public class SidebarFilterArgs {
    public Subject? SelectedSubject { get; set; }
    public string? FilterArgs { get; set; }
}
