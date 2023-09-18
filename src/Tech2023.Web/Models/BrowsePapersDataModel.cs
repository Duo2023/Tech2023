using Tech2023.DAL.Models;

namespace Tech2023.Web.Models;
public class BrowsePapersDataModel
{
    public Subject SelectedSubject { get; } = new();
    public string SelectedCategory { get; } = "Year";
    public List<string> Categories { get; set; } = new();
    public string? FilterArgs { get; set; }
    public Resource? SelectedPaper {get; set; }
}
